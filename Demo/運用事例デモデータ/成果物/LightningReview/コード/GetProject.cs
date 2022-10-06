#region プロジェクトの取得

/// <summary>
/// プロジェクトを取得する
/// </summary>
private void GetProjectData()
{
    // プロジェクトデータ取得(GET)用のURL
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/projects";

    // プロジェクトデータを取得する
    string data = GetProjectData(url, resource);

    // 出力するファイル名
    string fileName = @"C:\Users\sampleuser\Documents\ProjectList.xml";

    // 取得したプロジェクトデータをファイルに出力する
    Encoding utf8 = Encoding.GetEncoding("UTF-8");
    StreamWriter sw = new StreamWriter(fileName, false, utf8);
    sw.Write(data);
    sw.Close();
}


/// <summary>
/// プロジェクトデータを取得する
/// </summary>
/// <param name="url">API実行用URL</param>
/// <param name="resource">API実行用リソース</param>
/// <returns>プロジェクトデータ(XML)</returns>
private string GetProjectData(string url, string resource)
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
    
    // レスポンスを取得する
    var result = client.Execute(request);
    string ret = string.Empty;
    ret = result.Content;

    return ret;
}

#endregion プロジェクトの取得

#region WBSの取得

/// <summary>
/// WBSを取得する
/// </summary>
private void GetProjectWBSData()
{
    // プロジェクトデータ取得(GET)用のURL
    //  - ID 4のプロジェクトを指定
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/projects/{id}/wbsnodes";
    int projectId = 4;

    // WBSデータを取得する
    string data = GetProjectWBSData(url, resource, projectId);

    // 出力するファイル名
    string fileName = @"C:\Users\sampleuser\Documents\ProjectWBSList.xml";

    // 取得したプロジェクトデータをファイルに出力する
    Encoding utf8 = Encoding.GetEncoding("UTF-8");
    StreamWriter sw = new StreamWriter(fileName, false, utf8);
    sw.Write(data);
    sw.Close();
}


/// <summary>
/// WBSデータを取得する
/// </summary>
/// <param name="url">API実行用URL</param>
/// <param name="resource">API実行用リソース</param>
/// <param name="projectId">対象のプロジェクト</param>
/// <returns>プロジェクトデータ(XML)</returns>
private string GetProjectWBSData(string url, string resource, int projectId)
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

    request.AddUrlSegment("id", projectId.ToString());

    // レスポンスを取得する
    var result = client.Execute(request);
    string ret = string.Empty;
    ret = result.Content;

    return ret;
}

#endregion WBSの取得

#region プロジェクトの作成

/// <summary>
/// プロジェクトを新規に作成する
/// </summary>
private void AddProjectData()
{
    // プロジェクトデータ追加(POST)用のURL
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc/";
    string resource = "/projects";

    // 追加するプロジェクトのデータを格納したCSVファイルを指定する
    string csvProject = @"C:\Users\sampleuser\Documents\Projects.csv";

    // 各プロジェクトに登録するメンバーのアカウントIDを指定する
    int[] members = new int[] { 4, 30, 31 };

    // プロジェクトWBS（標準WBS）を定義する
    //   WBSの構成は以下の通り
    //     + 外部仕様
    //      -- 外部仕様書作成
    //      -- 外部仕様レビュー
    XElement xmlWBS = new XElement("wbsnodes",
        new XElement("wbsnode",
            new XElement("name", "外部仕様"),
            new XElement("code", "SPEC"),
            new XElement("kind", "taskPackage"),
            new XElement("children",
                new XElement("wbsnode",
                    new XElement("name", "外部仕様書作成"),
                    new XElement("code", "SPEC-01"),
                    new XElement("kind", "task")
                ),
                new XElement("wbsnode",
                    new XElement("name", "外部仕様レビュー"),
                    new XElement("code", "SPEC-02"),
                    new XElement("kind", "task")
                )
            )
        )
    );

    // プロジェクトを作成する
    ArrayList ret = AddProjectData(url, resource, csvProject, members, xmlWBS);

    // 処理結果（作成したプロジェクトのID）を画面に出力する
    string displayText = string.Empty;
    foreach (string id in ret)
    {
        if (displayText == string.Empty)
        {
            displayText = id;
        }
        else
        {
            displayText = string.Concat(displayText, ", ", id);
        }
    }

    // （labelResultは画面上に設定されたオブジェクト）
    labelResult.Text = "プロジェクトの作成が完了しました。ID = " + displayText;
}

/// <summary>
/// プロジェクトを新規に作成する
/// </summary>
/// <param name="url">API実行用URL（プロジェクト作成）</param>
/// <param name="resource">API実行用リソース（プロジェクト作成）</param>
/// <param name="csvProject">作成するプロジェクトデータ(CSVファイル)</param>
/// <param name="members">各プロジェクトに登録するメンバー（アカウントID）</param>
/// <param name="xmlWBS">各プロジェクトに登録するWBS(XML)</param>
/// <returns>作成したプロジェクトのID</returns>
private ArrayList AddProjectData(string url, string resource, string csvProject, int[] members, XElement xmlWBS)
{
    // レスポンス（追加したIDのリスト）を格納するための配列
    ArrayList IdList = new ArrayList();

    // プロジェクト情報が格納されたCSVファイルを読み込む
    StreamReader csvsr = new StreamReader(csvProject, Encoding.GetEncoding("Shift_JIS"));

    // 1行ずつデータを追加する
    while (csvsr.EndOfStream == false)
    {
        string line = csvsr.ReadLine();
        string[] fields = line.Split(',');

        // ==========================
        // ====== プロジェクト ======
        // ==========================

        // 送信用のXMLデータを作成する
        // プロジェクトのCSVファイルには以下の順序で定義されているものとする
        //   プロジェクト名、プロジェクトコード、マネージャID、計画開始日、計画終了日
        XElement xml = new XElement("project",
            new XElement("name", fields[0]),
            new XElement("code", fields[1]),
            new XElement("managerId", fields[2]),
            new XElement("plannedStartDate", fields[3]),
            new XElement("plannedFinishDate", fields[4])
        );

        // プロジェクトを作成する
        int id = ExecuteAddProjectRequest(url, resource, -1, xml, Method.POST);

        IdList.Add(id.ToString()); // 追加したIDをリストに追加しておく

        // ======================
        // ====== メンバー ======
        // ======================

        // 各プロジェクトに共通のメンバーを登録する
        foreach (int accountId in members)
        {
            // 各アカウントのXMLデータを作成する
            xml = new XElement("member",
                    new XElement("accountId", accountId.ToString())
            );

            // メンバー追加(POST)用のURL
            string resourceMember = "/projects/{id}/members";

            // メンバーを登録する
            ExecuteAddProjectRequest(url, resourceMember, id, xml, Method.POST);
        }

        // =================
        // ====== WBS ======
        // =================

        // WBS追加(PUT)用のURL（ルートノード直下に追加する）
        string resourceWBS = "/projects/{id}/wbsnodes/massUpdate/-1?insertMode=children";

        // WBSを登録する
        ExecuteAddProjectRequest(url, resourceWBS, id, xmlWBS, Method.PUT);
    }

    return IdList;
}

/// <summary>
/// プロジェクト新規作成のリクエストを実行する
/// </summary>
/// <param name="url">API実行用URL</param>
/// <param name="resource">API実行用リソース</param>
/// <param name="projectId">対象のプロジェクト(プロジェクト作成時は -1)</param>
/// <param name="xml">作成するデータ(XML)</param>
/// <param name="method">実行するWeb APIのメソッド(POST, PUT)</param>
/// <returns>追加したオブジェクトのID</returns>
private int ExecuteAddProjectRequest(string url, string resource, int projectId, XElement xml, Method method)
{
    // 接続に使用するログイン名とパスワードを指定する
    string user = "okamoto";
    string password = string.Empty; // 空文字
    
    // リクエストを作成する
    var client = new RestClient(url);
    var request = new RestRequest(resource, method);
    client.PreAuthenticate = true;
    request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
    request.AddHeader("Content-Type", "application/xml");
    request.Credentials = new NetworkCredential(user, password);

    if (projectId != -1)
    {
        request.AddUrlSegment("id", projectId.ToString());
    }
    request.AddXmlBody(xml);

    // レスポンスを取得する
    var result = client.Execute(request);

    // 追加したID情報を戻り値とする（POSTの場合のみ）
    int ret = 0;
    if (method == Method.POST)
    {
        ret = Convert.ToInt32(result.Content.Trim('"'));
    }

    return ret;
}

#endregion プロジェクトの作成