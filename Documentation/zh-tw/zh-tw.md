# Client DataBase 本地資料讀取系統
本地資料讀取系統能方便讀取企劃 Excel 表格，將會自動產生相應程式碼與 [ScriptableObject](https://unity3d.com/cn/learn/tutorials/modules/beginner/live-training-archive/scriptable-objects) 資源檔。

<img src="https://github.com/k79k06k02k/ClientDataBase/blob/master/Documentation/01_Excel%20to%20ScriptableObject%20Asset.png">
<br><br><br>


## 資料夾結構
```
Assets/ClientDataBase/
    ├── GameTable/    --- Excel資料表(txt)
    ├── Generate/     --- 自動產出之資源與程式碼
        ├── Resources/    --- Scriptable 資源
        ├── Scriptable/   --- Scriptable 程式碼
        └── TableClass/   --- 資料列類別
    ├── Scripts/      --- 相關程式碼
        ├── Base/         --- 基底類別
        ├── Config/       --- 配置設定
        ├── Editor/       --- 編輯器工具
        └── Utility/      --- 常用工具
    └── Templates/    --- 程式碼版型
```

:warning: Generate資料夾 是由工具自動產生
<br><br><br>

## 配置檔案
方便修改資源路徑、類別名稱
```
Assets/ClientDataBase/Client DataBase Config.asset
```
<img src="https://github.com/k79k06k02k/ClientDataBase/blob/master/Documentation/02_Client%20DataBase%20Config.PNG" height="450">

| 分類 | 名稱 | 描述 |
| ------ | ----------- | ----------- |
| Check   ||
|| Game Table Check   |  檢查是否是 .txt 資料表，判斷開頭字串是否相符 |
| Path   | | |
|| ROOT | 根目錄 |
|| Script Templates Path | 程式碼版型路徑 |
|| Game Table Path | 資料表路徑 |
|| Table Class Path | "自動產出" 資料類別路徑 |
|| Scriptable Asset Path | "自動產出" Scriptable 資源路徑 |
|| Scriptable Scripts Path | "自動產出" Scriptable 程式碼路徑 |
|| Scriptable Editor Path | "自動產出" Scriptable 編輯程式碼路徑 |
| Name   | | |
|| Class Name Prefix | "自動產出" 資料類別名稱 |
|| Scriptable Asset Suffix | "自動產出" Scriptable 資源名稱 |
|| Scriptable Script Suffix | "自動產出" Scriptable 程式碼名稱 |
|| Scriptable Editor Suffix | "自動產出" Scriptable 編輯程式碼名稱 |
<br><br><br>

## 支援資料型態
+ string
+ int
+ long
+ float
+ double
+ bool
+ Vector2
+ Vector3
+ string[]
+ int[]
+ long[]
+ float[]
+ double[]
+ bool[]
+ Vector2[]
+ Vector3[]
<br><br><br>

## Excel 資料表格式
+ [一般 資料表格式範例](https://docs.google.com/spreadsheets/d/1jEI_wGavArBLVHQY89_w7iMdoyuVVtkLDYYXrI_lZsw/edit#gid=1809851094)

+ [陣列 資料表格式範例](https://docs.google.com/spreadsheets/d/1jEI_wGavArBLVHQY89_w7iMdoyuVVtkLDYYXrI_lZsw/edit#gid=0)

<br>
:warning: 幾個要注意的地方
+ 表格 "第一列" 為中文名稱
+ 表格 "第二列" 為變數名稱
+ 表格 "第三列" 為資料型態
+ 表格 "第一行" id 為讀取資料每列的 Key
+ bool類型：T為true，F為false
+ Vector2、Vector3類型：用逗號分隔
+ 陣列表示方法：第二列變數名稱後面加入[]，相同名稱為同個陣列數值
<br><br><br>

## 使用方式
1. 從 Excel 或是 Google Excel 匯出 Tab分隔資料 (.tsv)
2. 附檔名改為 .txt
3. 將 .txt 檔案放入 Assets\ClientDataBase\GameTable 中
4. 選擇資源產生方式
	+ 將 GameTable 下所有 .txt 表格資料產出資源
	  	Unity Menu -> Assets -> Client DataBase -> Update All
		
	+ 使用 視窗工具 選擇檔案後，建立或是更新資源
	  	Unity Menu -> Assets -> Client DataBase -> Window
		
      <img src="https://github.com/k79k06k02k/ClientDataBase/blob/master/Documentation/03_Client%20DataBase%20Window.PNG" height="350">
	 
	 頁籤 Create：選擇一個或多個 .txt 表格資料後，按下 Create 按鈕建立資源
	 
	 頁籤 Update：選擇一個或多個 資料表 ScriptableObject 資源後，按下 Update 按鈕重新從 .txt 表格 再次更新資料
	 
5. 將會自動產生相應程式碼與 ScriptableObject 資源
6. 讀取 ScriptableObject 資源後，呼叫以下方法取得每列資料
```cs
public Table[FileName] GetData(string id)
```
<br><br><br>

## 範例實作 (使用Google Excel)
1. Excel 資料表準備
	+ [範例資料表](https://docs.google.com/spreadsheets/d/1jEI_wGavArBLVHQY89_w7iMdoyuVVtkLDYYXrI_lZsw/edit#gid=457664276)
	
2. 匯出 Tab分隔資料 (.tsv)
   檔案 -> 下載 -> 定位鍵分隔值檔案(.tsv，目前工作表)
   
  <img src="https://github.com/k79k06k02k/ClientDataBase/blob/master/Documentation/04_Sample_1.png" height="450">
  
3. 附檔名改為 .txt，並將 .txt 檔案放入 Assets\ClientDataBase\GameTable 中

	<img src="https://github.com/k79k06k02k/ClientDataBase/blob/master/Documentation/05_Sample_2.png">

4. 工具自動產出程式碼 & 資源，期間會跳出視窗處理

	Unity Menu -> Assets -> Client DataBase -> Update All
	
	處理完成後會產生Generate資料夾，檔案結構如下
  ```
  Assets/ClientDataBase/
      ├── Generate/     
          ├── Resources/    
              └── ClientDataBase/   
                  └── TableSampleAsset.asset    --- Scriptable 資源
  
          ├── Scriptable/  
              ├── Editor/   
                  └── TableSampleScriptableEditor.cs    --- Scriptable 編輯程式碼
              └── Script/   
                  └── TableSampleScriptable.cs    --- Scriptable 程式碼
  
          └── TableClass/
              └── TableSample.cs    --- 資料類別
  ```

5. 讀取 ScriptableObject 資源，使用 Key 取得資料

```cs
using UnityEngine;

public class LoadTable : MonoBehaviour
{
    void Start()
    {
        TableSampleScriptable _TableSampleScriptable = Resources.Load<TableSampleScriptable>("ClientDataBase/TableSampleAsset");

        TableSample _TableSample = _TableSampleScriptable.GetData("Sample001");
        print(_TableSample.knowledge);
        print(_TableSample.pos[0]);
    }
}
```
<br><br>

#[作者網站](http://k79k06k02k.com/blog)<br>

[![CC-by-sa.png](http://k79k06k02k.com/Image/CC-by-sa.png)](http://creativecommons.org/licenses/by-sa/4.0/)<br>
本著作係採用創用 CC 姓名標示-相同方式分享 4.0 國際 授權條款授權。
