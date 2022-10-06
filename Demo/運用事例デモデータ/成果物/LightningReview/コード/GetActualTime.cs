#region 実績工数取得

/// <summary>
/// 実績工数データを取得する（実績単位）
/// </summary>
private void GetActualTime()
{
    // 実績工数データ取得用のURL（ID10のアカウントを指定）
    //   指定した期間のデータを実績単位で取得する
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/accounts/{id}/ActualTimes";
    int account = 10;
    string start = "2016-02-01";    // 取得対象期間（開始日）
    string finish = "2016-02-01";   // 取得対象期間（終了日）

    // 実績工数データを取得する
    string data = GetActualTime(url, resource, account, start, finish);

    // 出力するファイル名
    string fileName = @"C:\Users\sampleuser\Documents\ActualTime.xml";

    // 取得した実績工数データをファイルに出力する
    Encoding utf8 = Encoding.GetEncoding("UTF-8");
    StreamWriter sw = new StreamWriter(fileName, false, utf8);
    sw.Write(data);
    sw.Close();
}

/// <summary>
/// 実績工数データを取得する（日単位）
/// </summary>
private void GetDailyActualTime()
{
    // 実績工数データ取得用のURL（ID10のアカウントを指定）
    //   指定した期間のデータを日単位で取得する
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/accounts/{id}/dailyActualTimes";
    int account = 10;
    string start = "2016-02-01";    // 取得対象期間（開始日）
    string finish = "2016-02-01";   // 取得対象期間（終了日）

    // 実績工数データを取得する
    string data = GetActualTime(url, resource, account, start, finish);

    // 出力するファイル名
    string fileName = @"C:\Users\sampleuser\Documents\DailyActualTime.xml";

    // 取得した実績工数データをファイルに出力する
    Encoding utf8 = Encoding.GetEncoding("UTF-8");
    StreamWriter sw = new StreamWriter(fileName, false, utf8);
    sw.Write(data);
    sw.Close();
}

/// <summary>
/// 実績工数データを取得する
/// </summary>
/// <param name="url">API実行用URL</param>
/// <param name="resource">API実行用リソース</param>
/// <param name="accountId">対象のアカウント</param>
/// <param name="start">パラメータ：開始日</param>
/// <param name="finish">パラメータ：終了日</param>
/// <returns>実績工数データ</returns>
private string GetActualTime(string url, string resource, int accountId, string start, string finish)
{
    // 接続に使用するログイン名とパスワードを指定する
    string user = "okamoto";
    string password = string.Empty;

    // リクエストを作成する
    var client = new RestClient(url);
    var request = new RestRequest(resource, Method.GET);
    request.Credentials = new NetworkCredential(user, password);
    request.AddHeader("Content-Type", "application/xml");
    client.PreAuthenticate = true;

    // テンプレートパラメータ、クエリパラメータを追加する
    request.AddUrlSegment("id", accountId.ToString());
    request.AddQueryParameter("startDate", start);
    request.AddQueryParameter("finishDate", finish);

    var result = client.Execute(request);

    // レスポンスを取得する
    string ret = string.Empty;
    ret = result.Content;
    
    return ret;
}

#endregion 実績工数取得

#region 実績工数追加

/// <summary>
/// 実績工数データを追加する
/// </summary>
private void AddActualTime()
{
    // 実績工数データ追加(POST)用のURL（ID10のアカウントを指定）
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc/";
    string resource = "/accounts/{id}/actualTimes";
    int accountId = 10;

    // 対象アカウントについて追加する工数データを格納したCSVファイルを指定する
    string csvFile = @"C:\Users\sampleuser\Documents\ActualTime.csv";

    // 実績工数データを追加する
    ArrayList ret = AddActualTime(url, resource, accountId, csvFile);

    // 処理結果（追加した実績工数データ）を画面に出力する
    string displayText = string.Empty;
    foreach (string id in ret)
    {
        if (displayText == string.Empty)
        {
            displayText = string.Concat("ID = ", id.Trim('"'));
        }
        else
        {
            displayText = string.Concat(displayText, ", ", id.Trim('"'));
        }
    }
    labelResult.Text = "実績データの追加が完了しました。" + displayText;

}

/// <summary>
/// 実績工数データを新規に追加する
/// </summary>
/// <param name="url">API実行用URL</param>
/// <param name="resource">API実行用リソース</param>
/// <param name="accountId">対象のアカウント</param>
/// <param name="csvFile">実績工数データが格納されたCSVファイル名</param>
/// <returns>追加した実績IDのリスト</returns>
private ArrayList AddActualTime(string url, string resource, int accountId, string csvFile)
{
    // 接続に使用するログイン名とパスワードを指定する
    string user = "okamoto";
    string password = string.Empty;     // 空文字（サンプルデータの既定値）

    // 実績情報が格納されたCSVファイルを読み込む
    StreamReader csvsr = new StreamReader(csvFile, Encoding.GetEncoding("Shift_JIS"));

    // レスポンス（追加したIDのリスト）を格納するための配列
    ArrayList idList = new ArrayList();

    // 1行ずつデータを追加する
    while (csvsr.EndOfStream == false)
    {
        string line = csvsr.ReadLine();
        string[] fields = line.Split(',');

        // 各フィールドの値を取得する
        int projectId = Convert.ToInt32(fields[0]);     // プロジェクトID
        int taskId = Convert.ToInt32(fields[1]);        // タスクID
        string startTime = fields[2];                   // 開始日時
        string finishTime = fields[3];                  // 終了日時
        string memo = fields[4];                        // メモ

        // 送信用のXMLデータを作成する
        XElement xml = new XElement("actualTime",
            new XElement("projectId", projectId),
            new XElement("taskId", taskId),
            new XElement("startTime", startTime),
            new XElement("finishTime", finishTime),
            new XElement("memo", memo)
        );
        
        // リクエストを作成する
        var client = new RestClient(url);
        var request = new RestRequest(resource, Method.POST);
        client.PreAuthenticate = true;
        request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
        request.Credentials = new NetworkCredential(user, password);
        request.AddHeader("Content-Type", "application/xml");

        request.AddUrlSegment("id", accountId.ToString());
        request.AddXmlBody(xml);

        // レスポンスを取得する
        var result = client.Execute(request);
        idList.Add(result.Content); // 追加したIDをリストに追加しておく
    }
    return idList;
}

#endregion 実績工数追加

#region 実績工数追加（進捗率を含む）

/// <summary>
/// 実績工数データを追加する（進捗率を含む）
/// </summary>
private void AddActualTimeProgress()
{
    // 実績工数データ追加(POST)用のURL（ID10のアカウントを指定）
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc/";
    string resource = "/accounts/{id}/actualTimes";
    int accountId = 10;
    
    // 対象アカウントについて追加する工数データを格納したCSVファイルを指定する
    string csvFile = @"C:\Users\sampleuser\Documents\actualtime_progress.csv";

    // 実績工数データを追加する（進捗率を含む）
    ArrayList ret = AddActualTimeProgress(url, resource, accountId, csvFile);

    // 処理結果（追加した実績工数データ）を画面に出力する
    string displayText = string.Empty;
    foreach (string id in ret)
    {
        if (displayText == string.Empty)
        {
            displayText = string.Concat("ID = ", id.Trim('"'));
        }
        else
        {
            displayText = string.Concat(displayText, ", ", id.Trim('"'));
        }
    }
    labelResult.Text = "実績データの追加が完了しました。" + displayText;
}

/// <summary>
/// 実績工数データを新規に追加する（進捗率を含む）
/// </summary>
/// <param name="url">API実行用URL（実績工数追加用）</param>
/// <param name="resource">API実行用リソース（割り当てタスク情報更新用）</param>
/// <param name="accountId">対象のアカウント</param>
/// <param name="csvFile">進捗率を含む実績工数データが格納されたCSVファイル名</param>
/// <returns>追加した実績IDのリスト</returns>
private ArrayList AddActualTimeProgress(string url, string resource, int accountId, string csvFile)
{
    // 接続に使用するログイン名とパスワードを指定する
    string user = "okamoto";
    string password = string.Empty;     // 空文字（サンプルデータの既定値）

    // 実績情報が格納されたCSVファイルを読み込む
    StreamReader csvsr = new StreamReader(csvFile, Encoding.GetEncoding("Shift_JIS"));

    // レスポンス（追加したIDのリスト）を格納するための配列
    ArrayList idList = new ArrayList();

    // 1行ずつデータを追加する
    while (csvsr.EndOfStream == false)
    {
        string line = csvsr.ReadLine();
        string[] fields = line.Split(',');

        // 各フィールドの値を取得する
        int projectId = Convert.ToInt32(fields[0]);     // プロジェクトID
        int taskId = Convert.ToInt32(fields[1]);        // タスクID
        string startTime = fields[2];                   // 開始日時
        string finishTime = fields[3];                  // 終了日時
        string memo = fields[4];                        // メモ
        int progress = Convert.ToInt32(fields[5]);      // 進捗率

        // 送信用のXMLデータを作成する
        XElement xml = new XElement("actualTime",
            new XElement("projectId", projectId),
            new XElement("taskId", taskId),
            new XElement("startTime", startTime),
            new XElement("finishTime", finishTime),
            new XElement("memo", memo)
        );

        // リクエストを作成する
        var client = new RestClient(url);
        var request = new RestRequest(resource, Method.POST);
        client.PreAuthenticate = true;
        request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
        request.Credentials = new NetworkCredential(user, password);
        request.AddHeader("Content-Type", "application/xml");

        request.AddUrlSegment("id", accountId.ToString());
        request.AddXmlBody(xml);

        // レスポンスを取得する
        var result = client.Execute(request);
        idList.Add(result.Content); // 追加したIDをリストに追加しておく


        // タスクの進捗情報を登録する

        // 更新内容を定義する（割り当てタスクの進捗率を更新）
        xml = new XElement("assignedTask",
            new XElement("assignmentProgress", progress)
        );
        
        // リクエストを作成する
        string resourceAssignedTask = "/accounts/{accountId}/assignedProjects/{projectId}/assignedTasks/{taskId}";
        client = new RestClient(url);
        request = new RestRequest(resourceAssignedTask, Method.PUT);
        client.PreAuthenticate = true;
        request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
        request.Credentials = new NetworkCredential(user, password);
        request.AddHeader("Content-Type", "application/xml");

        request.AddUrlSegment("accountId", accountId.ToString());
        request.AddUrlSegment("projectId", projectId.ToString());
        request.AddUrlSegment("taskId", taskId.ToString());
        request.AddXmlBody(xml);

        // レスポンスを取得する
        result = client.Execute(request);
        string ret = result.Content;
    }
    return idList;
}

#endregion 実績工数追加（進捗率を含む）