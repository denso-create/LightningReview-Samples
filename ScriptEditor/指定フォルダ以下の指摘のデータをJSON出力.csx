// 本スクリプトは、コマンドラインからスクリプトを実行する想定であり、
// 任意のフォルダ以下のレビューファイルの指摘データを集計し、JSONファイルを出力する。
// 出力したJSONファイルは、BIツールなどで読み込んで分析する対象とする。

// 結果を出力するファイルを指定(サンプルとしてデスクトップに Output.json を出力)
var outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Output.json");

// 指摘のデータのうち、出力対象とするフィールドを定義したクラス
class IssueData
{
    // 指摘ID
    public string Id {get; set;}
    // ステータス
    public string Status {get; set;}
    // 重大度
    public string Importance {get; set;}
    // 検出工程
    public string DetectionActivity {get; set;}
    // 原因工程
    public string InjectionActivity {get; set;}
    // 修正者
    public string AssignedTo {get; set;}
}

// 指定フォルダ以下のレビューファイルの指摘データを集計する(サンプルとしてデスクトップ以下のレビューファイルを集計)
var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
var reviewFilePaths = System.IO.Directory.GetFiles(folderPath, "*.revx", SearchOption.AllDirectories);
var issueDataList = new List<IssueData>();
var service = App.GetReviewFileService();
foreach(var reviewFilePath in reviewFilePaths)
{
    try
    {
        var review = service.OpenReview(reviewFilePath);
        foreach(var issue in review.GetAllIssues())
        {
            issueDataList.Add(new IssueData()
            {
                Id = issue.Id,
                Status = issue.Status,
                Importance = issue.Importance,
                DetectionActivity = issue.DetectionActivity,
                InjectionActivity = issue.InjectionActivity,
                AssignedTo = issue.AssignedTo,
            });
        }
        service.CloseReview(review);
    }
    catch
    {
        // 例外が起きたファイルは何もしない
    }
}

// JSONにシリアライズしてファイル出力する
var serializer = new JsonSerializer()
{
    Formatting = Formatting.Indented,
};

using(TextWriter writer = File.CreateText(outputPath))
{
    serializer.Serialize(writer, issueDataList);
}

// レビューファイルを開いていない、かつ、ダーティ状態でない場合は、
// スクリプトの実行のためにレビューウィンドウを起動したと判断してレビューウィンドウを閉じる
var window = App.ActiveReviewWindow;
if(window != null && string.IsNullOrEmpty(window.Review.FilePath) && !window.Review.IsDirty)
{
    App.Quit();
}