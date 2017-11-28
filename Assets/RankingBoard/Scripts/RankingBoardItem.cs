using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Anz
{
	public class RankingBoardItem: MonoBehaviour
	{
		#region "SerializeFields"
		[SerializeField] private Text rank;
		[SerializeField] private Text nickname;
		[SerializeField] private Text score;
		[SerializeField] private Image separator;
		#endregion
		
		#region "Fields"
		#endregion
		
		#region "Properties"
		#endregion
		
		#region "Lifecycles"
		private void Awake()
		{
		}
		
		private void Start()
		{
		}
		#endregion
		
		#region "Events"
		#endregion
		
		#region "Methods"
		public void SetAppearance(BoardAppearance appearance)
		{
			if (appearance.Font != null) {
				this.rank.font = appearance.Font;
				this.nickname.font = appearance.Font;
				this.score.font = appearance.Font;
			}
			this.rank.color = appearance.TextColor;
			this.rank.fontSize = appearance.RankingFontSize;
			this.nickname.color = appearance.TextColor;
			this.nickname.fontSize = appearance.RankingFontSize;
			this.score.color = appearance.TextColor;
			this.score.fontSize = appearance.RankingFontSize;
			this.separator.color = appearance.SeparatorColor;
		}

		public void Configure(int rank, string nickname, string score)
		{
			this.rank.text = rank.ToString();
			this.nickname.text = nickname;
			this.score.text = score;
		}
		#endregion
		
	}
}