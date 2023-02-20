// 未修正の指摘が数十件ある場合に、優先して修正すべき指摘だけを表示するようにフィルタできます。
// サンプルとして、『優先度が高』、『ステータスが未修正』、『修正者が未定』の条件を満たす指摘のみを表示するフィルタを適用しています。

var issues = App.ActiveReviewWindow.Review.GetAllIssues();

var stringBuilder = new StringBuilder();
foreach(var issue  in issues)
{
  if (issue.Status == "NotRevised" && issue.AssignedTo == "(未定)" && issue.Priority == "High")
  {
    stringBuilder.Append(issue.Id);
    stringBuilder.Append(",");
  }
}
var issueIds = stringBuilder.ToString().TrimEnd(',');
App.ActiveReviewWindow.ApplyIdFilter(issueIds);
return $"指摘ID:{issueIds}を優先して修正してください。"; 