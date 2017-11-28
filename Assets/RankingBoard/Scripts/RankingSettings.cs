using UnityEngine;

namespace Anz
{
	public class RankingSettings: MonoBehaviour
	{
		#region "SerializeFields"
		[Tooltip("NCMBでランキングを扱っているクラス名")]
		public string ClassName = "RankingBoard";
		[Tooltip("ニックネームを保存するフィールド名")]
		public string PlayerColumn = "nickname";
		[Tooltip("スコアを保存するフィールド名")]
		public string ScoreColumn = "socre";
		[Tooltip("昇順フラグ（trueで昇順/falseで降順）")]
		public bool IsAscending = false;
		#endregion
	}
}