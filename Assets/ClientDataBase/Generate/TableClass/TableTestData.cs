/*****************************************************************************/
/****************** Auto Generate Script, Do Not Modify! *********************/
/*****************************************************************************/
using UnityEngine;
using System.Collections;
using ClientDataBase;
 
[System.Serializable]
public class TableTestData : TableClassBase
{
	/// <summary>
    /// 索引
    /// </summary>
    public int id { get { return _id; } set { _id = value; } }
	[SerializeField] 
	private int _id;

	/// <summary>
    /// 名稱
    /// </summary>
    public string name { get { return _name; } set { _name = value; } }
	[SerializeField] 
	private string _name;

	/// <summary>
    /// 技能1, 技能2
    /// </summary>
    public int[] skill { get { return _skill; } set { _skill = value; } }
	[SerializeField] 
	private int[] _skill;

	/// <summary>
    /// 管理能力
    /// </summary>
    public float[] ability1 { get { return _ability1; } set { _ability1 = value; } }
	[SerializeField] 
	private float[] _ability1;

	/// <summary>
    /// 公關能力, 策劃能力
    /// </summary>
    public int[] ability2 { get { return _ability2; } set { _ability2 = value; } }
	[SerializeField] 
	private int[] _ability2;


}