using UnityEngine;
using UnityEngine.UI;

namespace Anz
{
	public class BoardAppearance: MonoBehaviour
	{
		[Space(4), Tooltip("全テキストに反映されるFont\nWebGL出力の場合デフォルトのArialだと表示されないのでFont指定忘れずに")]
		public Font Font;
		[Tooltip("全テキストの文字色に反映される")]
		public Color TextColor = Color.black;

		[Space(16)]
		public string ScorePrefix = "Score: ";
		public string NicknamePlaceholder = "ニックネーム入力";
		public string SendButtonText = "送信";
		public string CloseButtonText = "閉じる";
		[Space(8)]
		[Tooltip("ニックネームInputFieldの背景画像")]
		public Sprite NicknameBackground;
		[Tooltip("送信ボタンの背景画像")]
		public Sprite SendButtonBackground;
		[Tooltip("閉じるボタンの背景画像")]
		public Sprite CloseButtonBackground;
		[Space(8)]
		[Tooltip("ニックネームInputFieldの背景色")]
		public Color NicknameBackgroundColor = Color.white;
		[Tooltip("送信ボタンテキスト色")]
		public Color SendButtonTextColor = Color.black;
		[Tooltip("送信ボタン背景画像色")]
		public Color SendButtonBackgroundColor = Color.white;
		[Tooltip("フォームエリアの枠線色")]
		public Color FormBorderColor = Color.white;
		[Tooltip("フォームエリアの背景色")]
		public Color FormBackgroundColor = Color.white;
		[Tooltip("閉じるボタンテキスト色")]
		public Color CloseButtonTextColor = Color.black;
		[Tooltip("閉じるボタン背景画像色")]
		public Color CloseButtonBackgroundColor = Color.white;

		[Space(16), Tooltip("ランキングアイテムのフォントサイズ")]
		public int RankingFontSize = 14;
		[Tooltip("ランキングアイテムのセパレーター色")]
		public Color SeparatorColor = Color.black;
		[Tooltip("ランキングエリアの枠線色")]
		public Color RankingBorderColor = Color.white;
		[Tooltip("ランキングエリアの背景色")]
		public Color RankingBackgroundColor = Color.white;
	}
}