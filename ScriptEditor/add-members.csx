// 本スクリプトは、プロジェクトで利用しているすべてのレビューファイルのテンプレートに対して、
// 新しく業務配属されたメンバを、レビューファイルのメンバ一覧に一括で追加するために使用します。

// 以下に追加したいメンバ名を設定
var memberNames = new [] {"近藤"};

// 以下に追加したいテンプレートファイルがあるフォルダを設定
var templateFolder = @"C:\Git\lightning-review-operational-test\Docs\エンジニアリング\Current\07_ピアレビュー\Templates";

var directoryInfo = new DirectoryInfo(templateFolder);
var files = directoryInfo.GetFiles("*.revx");
var service = App.GetReviewFileService();
foreach(var file in files)
{
    var review = service.OpenReview(file.FullName);

    foreach(var memberName in memberNames)
    {
        // メンバ追加
        var newMember = review.ReviewSetting.AddMember(memberName);
        newMember.Reviewee = true;
        newMember.Reviewer = true;
        newMember.Moderator = true;
    }

    // 保存して閉じる
    service.SaveReview(file.FullName, review);
    service.CloseReview(review);
}