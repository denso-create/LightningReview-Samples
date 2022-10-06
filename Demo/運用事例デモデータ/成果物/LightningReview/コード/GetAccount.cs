#region アカウントの取得

/// <summary>
/// アカウントを取得する
/// </summary>
private void GetAccountData()
{
    // アカウント取得(GET)用のURL
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/system/accounts";
    
    // アカウント情報を取得する
    string data = GetAccountData(url, resource);

    // 出力するファイル名
    string fileName = @"C:\Users\sampleuser\Documents\AccountList.xml";

    // 取得したアカウント情報をファイルに出力する
    Encoding utf8 = Encoding.GetEncoding("UTF-8");
    StreamWriter sw = new StreamWriter(fileName, false, utf8);
    sw.Write(data);
    sw.Close();
}

/// <summary>
/// アカウントを取得する
/// </summary>
/// <param name="url">API実行用URL</param>
/// <param name="resource">API実行用リソース</param>
/// <returns>アカウントデータ</returns>
private string GetAccountData(string url, string resource)
{
    // 接続に使用するログイン名とパスワードを指定する
    string user = "okamoto";
    string password = string.Empty; // 空文字

    // リクエストを作成する
    var client = new RestClient(url);
    var request = new RestRequest(resource, Method.GET);
    request.Credentials = new NetworkCredential(user, password);
    request.AddHeader("Content-Type", "application/xml");
    client.PreAuthenticate = true;

    var result = client.Execute(request);

    // レスポンスを取得する
    string ret = string.Empty;
    ret = result.Content;

    return ret;
}

#endregion アカウントの取得

#region アカウントの更新

/// <summary>
/// アカウントを更新する
/// </summary>
private void UpdateAccountData()
{
    // アカウント更新(PUT)用のURL
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/system/accounts/{id}";

    // 更新内容を定義する
    // （組織・役割・コスト単価を変更）
    int accountId = 12; // 更新対象のアカウントID
    XElement xml = new XElement("account",
        new XElement("name", "植田 信貴"),
        new XElement("code", "009"),
        new XElement("loginName", "ueda"),
        new XElement("sectionId", 4),
        new XElement("roleId", 3),
        new XElement("unitCost", 8000)
    );

    // アカウントのデータを更新する
    string ret = UpdateAccountData(url, resource, accountId, xml);

    // 処理結果を画面に出力する
    // （labelResultは画面上に設定されたオブジェクト）
    labelResult.Text = "アカウントの更新が完了しました。";

    // 更新後のアカウント一覧情報を取得する
    GetAccountData();
}

/// <summary>
/// アカウントを更新する
/// </summary>
/// <param name="url">API実行用URL</param>
/// <param name="resource">API実行用リソース</param>
/// <param name="accountId">対象のアカウント</param>
/// <param name="xml">更新データ（XML形式）</param>
/// <returns></returns>
private string UpdateAccountData(string url, string resource, int accountId, XElement xml)
{
    // 接続に使用するログイン名とパスワードを指定する
    string user = "okamoto";
    string password = string.Empty; // 空文字

    // XMLデータを送信可能な型に変換する
    byte[] data = Encoding.UTF8.GetBytes(xml.ToString());

    // リクエストを作成する（アカウントデータを更新）
    var client = new RestClient(url);
    var request = new RestRequest(resource, Method.PUT);
    client.PreAuthenticate = true;
    request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
    request.AddHeader("Content-Type", "application/xml");
    request.Credentials = new NetworkCredential(user, password);

    request.AddUrlSegment("id", accountId.ToString());
    request.AddXmlBody(xml);

    var result = client.Execute(request);

    // レスポンスを取得する
    string ret = result.ErrorMessage;

    // 正常に終了した場合は空文字が返る
    return ret;
}

#endregion アカウントの更新

#region アカウントの追加

/// <summary>
/// アカウントを追加する
/// </summary>
private void AddAccountData()
{
    // アカウント追加(POST)用のURL
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/system/accounts";

    // 追加するアカウントのデータを格納したCSVファイルを指定する
    string csvFile = @"C:\Users\sampleuser\Documents\accounts.csv";

    // アカウントを追加する
    ArrayList ret = AddAccountData(url, resource, csvFile);

    // 処理結果（追加したアカウントのID）を画面に出力する
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
    labelResult.Text = "アカウントの追加が完了しました。" + displayText;

    // 追加後のアカウント一覧情報を取得する
    GetAccountData();
}

/// <summary>
/// アカウントを新規に追加する
/// </summary>
/// <param name="url">API実行用URL</param>
/// <param name="resource">API実行用リソース</param>
/// <param name="csvFile">追加アカウントのデータが格納されたCSVファイル名</param>
/// <returns>追加したアカウントIDのリスト</returns>
private ArrayList AddAccountData(string url, string resource, string csvFile)
{
    // 接続に使用するログイン名とパスワードを指定する
    string user = "okamoto";
    string password = string.Empty; // 空文字

    // アカウント情報が格納されたCSVファイルを読み込む
    StreamReader csvsr = new StreamReader(csvFile, Encoding.GetEncoding("Shift_JIS"));

    // レスポンス（追加したIDのリスト）を格納するための配列
    ArrayList idList = new ArrayList();

    // 1行ずつデータを追加する
    while(csvsr.EndOfStream == false)
    {
        string line = csvsr.ReadLine();
        string[] fields = line.Split(',');

        // 各フィールドの値を取得する
        string name = fields[0];    // アカウント名
        string code = fields[1];    // アカウントコード
        string loginName = fields[2];   // ログイン名
        int sectionId = Convert.ToInt32(fields[3]); // 組織ID
        int roleId = Convert.ToInt32(fields[4]);    // 役割ID
        int unitCost = Convert.ToInt32(fields[5]);  // コスト単価

        // 送信用のXMLデータを作成する
        XElement xml = new XElement("account",
            new XElement("name", name),
            new XElement("code", code),
            new XElement("loginName", loginName),
            new XElement("sectionId", sectionId),
            new XElement("roleId", roleId),
            new XElement("unitCost", unitCost)                    
        );
        
        // リクエストを作成する
        var client = new RestClient(url);
        var request = new RestRequest(resource, Method.POST);
        client.PreAuthenticate = true;
        request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
        request.Credentials = new NetworkCredential(user, password);
        request.AddHeader("Content-Type", "application/xml");

        request.AddXmlBody(xml);

        var result = client.Execute(request);
        
        // レスポンスを取得する
        idList.Add(result.Content); // 追加したIDをリストに追加しておく
    }

    return idList;
}

#endregion  アカウントの追加