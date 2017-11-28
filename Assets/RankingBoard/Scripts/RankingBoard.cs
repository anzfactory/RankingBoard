using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Xyz.AnzFactory.UI;
using NCMBRest;
using MiniJSON;

namespace Anz
{
	[RequireComponent(typeof(NCMBRestController))]
	[RequireComponent(typeof(RankingSettings))]
	[RequireComponent(typeof(BoardAppearance))]
	public class RankingBoard: MonoBehaviour, ANZListView.IDataSource
	{
		#region "SerializeFields"
		[SerializeField] private Image formBorder;
		[SerializeField] private Image formBackground;
		[SerializeField] private Text scoreLabel;
		[SerializeField] private InputField nicknameField;
		[SerializeField] private Button sendButton;
		[SerializeField] private ANZListView listView;
		[SerializeField] private Button closeButton;
		[SerializeField] private RankingBoardItem itemTemplate;
		#endregion
		
		#region "Fields"
		private List<Dictionary<string, object>> items;
		private NCMBRestController restController;
		private RankingSettings settings;
		private BoardAppearance appearance;
		private float currentScore;
		#endregion
		
		#region "Properties"
		#endregion
		
		#region "Lifecycles"
		private void Awake()
		{
			this.items = new List<Dictionary<string, object>>();
			this.restController = this.gameObject.GetComponent<NCMBRestController>();
			this.settings = this.gameObject.GetComponent<RankingSettings>();
			this.appearance = this.gameObject.GetComponent<BoardAppearance>();
			this.SetupListView();
		}
		
		private void Start()
		{
			this.gameObject.SetActive(false);
			this.SetAppearance();
		}
		#endregion

		#region "Events"
		public void OnTapSend()
		{
			this.gameObject.SetActive(true);
			var nickname = "No name";
			if (!string.IsNullOrEmpty(this.nicknameField.text)) {
				nickname = this.nicknameField.text;
			}

			StartCoroutine(this.SendScore(nickname, this.currentScore, () => {
				this.Reload();
			}));
		}
		public void OnTapClose()
		{
			this.Close();
		}
		#endregion
		
		#region "Methods"
		public void Open(float currentScore)
		{
			this.currentScore = currentScore;
			this.gameObject.SetActive(true);
			this.scoreLabel.text = string.Format("{0}{1}", this.appearance.ScorePrefix, currentScore.ToString());
			this.Reload();
		}

		public void Close()
		{
			this.gameObject.SetActive(false);
		}

		private void SetupListView()
		{
			this.listView.DataSource = this;
		}

		private void SetAppearance()
		{
			var placeholder = this.nicknameField.placeholder.GetComponent<Text>();
			var buttonText = this.sendButton.GetComponentInChildren<Text>();
			var closeButtonText = this.closeButton.GetComponentInChildren<Text>();
			if (this.appearance.Font != null) {
				this.scoreLabel.font = this.appearance.Font;
				this.nicknameField.textComponent.font = this.appearance.Font;
				placeholder.font = this.appearance.Font;
				buttonText.font = this.appearance.Font;
				closeButtonText.font = this.appearance.Font;
			}

			this.scoreLabel.color = this.appearance.TextColor;
			this.nicknameField.textComponent.color = this.appearance.TextColor;
			placeholder.text = this.appearance.NicknamePlaceholder;
			var placeholderColor = this.appearance.TextColor;
			placeholderColor.a = 0.4f;
			placeholder.color = placeholderColor;
			var placeholderImage = this.nicknameField.GetComponent<Image>();
			if (placeholderImage != null) {
				placeholderImage.sprite = this.appearance.NicknameBackground;
				placeholderImage.color = this.appearance.NicknameBackgroundColor;
			}
			buttonText.color = this.appearance.SendButtonTextColor;
			buttonText.text = this.appearance.SendButtonText;
			var buttonImage = this.sendButton.GetComponent<Image>();
			if (buttonImage != null) {
				buttonImage.sprite = this.appearance.SendButtonBackground;
				buttonImage.color = this.appearance.SendButtonBackgroundColor;
			}
			this.formBorder.color = this.appearance.FormBorderColor;
			this.formBackground.color = this.appearance.FormBackgroundColor;
			this.listView.SetBorderColor(this.appearance.RankingBorderColor);
			this.listView.SetBackgroundColor(this.appearance.RankingBackgroundColor);
			closeButtonText.text = this.appearance.CloseButtonText;
			closeButtonText.color = this.appearance.CloseButtonTextColor;
			var closeButtonImage = this.closeButton.GetComponent<Image>();
			if (closeButtonImage != null) {
				closeButtonImage.sprite = this.appearance.CloseButtonBackground;
				closeButtonImage.color = this.appearance.CloseButtonBackgroundColor;
			}
		}

		private void Reload() 
		{
			StartCoroutine(this.GetRanking(items => {
				this.items = items;
				this.listView.ReloadData(() => {
					this.listView.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
				});
			}));
		}

		private IEnumerator GetRanking(Action<List<Dictionary<string, object>>> callback)
		{
			NCMBDataStoreParamSet paramSet = new NCMBDataStoreParamSet();
			paramSet.Limit = 200;
			paramSet.SortColumn = string.Format("{0}{1}", this.settings.IsAscending ? "" : "-", this.settings.ScoreColumn);
			var iter = this.restController.Call(NCMBRestController.RequestType.GET, string.Format("classes/{0}", this.settings.ClassName), paramSet, null);

			yield return StartCoroutine(iter);

			var result = new List<Dictionary<string, object>>();
			var jsonStr = (string)iter.Current;
			var jsonObject = (Dictionary<string, object>)Json.Deserialize(jsonStr);
			if (jsonObject.ContainsKey("results")) {
				foreach (var item in (List<object>)jsonObject["results"]) {
					result.Add((Dictionary<string, object>)item);
				}
			}
			callback(result);
		}

		private IEnumerator SendScore(string nickname, float score, Action callback)
		{
			var parameters = new Dictionary<string, object>();
			parameters[this.settings.PlayerColumn] = nickname;
			parameters[this.settings.ScoreColumn] = score;
			NCMBDataStoreParamSet paramSet = new NCMBDataStoreParamSet();
			paramSet.FieldsAsJson = Json.Serialize(parameters);
			var iter = this.restController.Call(NCMBRestController.RequestType.POST, string.Format("classes/{0}", this.settings.ClassName), paramSet, null);

			yield return StartCoroutine(iter);

			callback();
		}
		#endregion

		#region "ANZListView.IDataSource"
		public int NumOfItems()
		{
			return this.items.Count;
		}
		public float HeightItem()
		{
			return this.itemTemplate.gameObject.GetComponent<RectTransform>().rect.height;
		}
		public GameObject ListViewItem(int index, GameObject item)
		{
			RankingBoardItem rankingBoardItem = null;
			if (item == null) {
				item = GameObject.Instantiate(this.itemTemplate.gameObject);
				rankingBoardItem = item.GetComponent<RankingBoardItem>();
				rankingBoardItem.SetAppearance(this.appearance);
			} else {
				rankingBoardItem = item.GetComponent<RankingBoardItem>();
			}
			var nickname = this.items[index].ContainsKey(this.settings.PlayerColumn) ? this.items[index][this.settings.PlayerColumn].ToString() : "no name";
			var score = this.items[index].ContainsKey(this.settings.ScoreColumn) ? this.items[index][this.settings.ScoreColumn].ToString() : "";
			rankingBoardItem.Configure(index + 1, nickname, score);
			return item;
		}
		#endregion
		
	}
}