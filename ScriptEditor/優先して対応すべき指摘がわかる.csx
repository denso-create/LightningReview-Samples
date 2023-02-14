// 優先して対応すべき指摘がわかるスクリプト
// 未修正かつ優先度高かつ修正者が未定の指摘をフィルタする
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