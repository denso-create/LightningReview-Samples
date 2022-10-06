#region WBSの一括作成

/// <summary>
/// WBSを一括作成する
/// </summary>
private void BatchAddWBSData()
{
    //  WBS一括作成用のURL
    //  - ID 4のプロジェクトのアウトライン番号1を指定
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/projects/{projectId}/wbsnodes/massUpdate/{idOrOutlinenumber}";
    int projectId = 4;
    string outlineNumber = "1";

    // 一括作成するWBSを定義する
    //   WBSの構成は以下の通り
    //   各タスクに、アカウントID 3, 4, 6を割り当てる
    //   + 設計
    //     + 基本設計
    //      -- 基本設計
    //      -- 基本設計レビュー
    //     + 詳細設計
    //      -- 詳細設計
    //      -- 詳細設計レビュー

    #region WBSの定義

    XElement xmlWBS = new XElement("wbsnodes",
        new XElement("wbsnode",
            new XElement("name", "設計"),
            new XElement("kind", "taskPackage"),
            new XElement("children",
                new XElement("wbsnode",
                    new XElement("name", "基本設計"),
                    new XElement("kind", "taskPackage"),
                    new XElement("children",
                        new XElement("wbsnode",
                            new XElement("name", "基本設計"),
                            new XElement("kind", "task"),
                            new XElement("assignments",
                                new XElement("assignment",
                                    new XElement("accountId", 3)
                                ),
                                new XElement("assignment",
                                    new XElement("accountId", 4)
                                ),
                                new XElement("assignment",
                                    new XElement("accountId", 6)
                                )
                            )
                        ),
                        new XElement("wbsnode",
                            new XElement("name", "基本設計レビュー"),
                            new XElement("kind", "task"),
                            new XElement("assignments",
                                new XElement("assignment",
                                    new XElement("accountId", 3)
                                ),
                                new XElement("assignment",
                                    new XElement("accountId", 4)
                                ),
                                new XElement("assignment",
                                    new XElement("accountId", 6)
                                )
                            )
                        )
                    )
                ),
                new XElement("wbsnode",
                    new XElement("name", "詳細設計"),
                    new XElement("kind", "taskPackage"),
                    new XElement("children",
                        new XElement("wbsnode",
                            new XElement("name", "詳細設計"),
                            new XElement("kind", "task"),
                            new XElement("plannedStartDate", "2016/04/01"),
                            new XElement("plannedFinishDate", "2016/04/01"),
                            new XElement("assignments",
                                new XElement("assignment",
                                    new XElement("accountId", 3)
                                ),
                                new XElement("assignment",
                                    new XElement("accountId", 4)
                                ),
                                new XElement("assignment",
                                    new XElement("accountId", 6)
                                )
                            )
                        ),
                        new XElement("wbsnode",
                            new XElement("name", "詳細設計レビュー"),
                            new XElement("kind", "task"),
                            new XElement("plannedStartDate", "2016/04/04"),
                            new XElement("plannedFinishDate", "2016/04/04"),
                            new XElement("assignments",
                                new XElement("assignment",
                                    new XElement("accountId", 3)
                                ),
                                new XElement("assignment",
                                    new XElement("accountId", 4)
                                ),
                                new XElement("assignment",
                                    new XElement("accountId", 6)
                                )
                            )
                        )
                    )
                )
            )
        )
    );

    #endregion WBSの定義

    // WBSデータを一括作成する
    BatchAddWBSData(url, resource, projectId, outlineNumber, xmlWBS);

    // WBSデータを取得する
    resource = "/projects/{id}/wbsnodes";
    string data = GetProjectWBSData(url, resource, projectId);

    // 出力するファイル名
    string fileName = @"C:\Users\sampleuser\Documents\ProjectWBSList.xml";

    // 取得したWBSデータをファイルに出力する
    Encoding utf8 = Encoding.GetEncoding("UTF-8");
    StreamWriter sw = new StreamWriter(fileName, false, utf8);
    sw.Write(data);
    sw.Close();
}

/// <summary>
/// WBSデータを一括作成する
/// </summary>
/// <param name="url">API実行用URL</param>
/// <param name="resource">API実行用リソース</param>
/// <param name="projectId">対象のプロジェクト</param>
/// <param name="idOrOutlinenumber">基準となるノードの位置</param>
/// <param name="xmlWBS">ノードデータ</param>
/// <returns>プロジェクトデータ(XML)</returns>
private string BatchAddWBSData(string url, string resource, int projectId, string idOrOutlinenumber, XElement xmlWBS)
{
    // 接続に使用するログイン名とパスワードを指定する
    string user = "okamoto";
    string password = string.Empty; // 空文字

    // リクエストを作成する
    var client = new RestClient(url);
    var request = new RestRequest(resource, Method.PUT);
    request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
    request.Credentials = new NetworkCredential(user, password);
    request.AddHeader("Content-Type", "application/xml");
    client.PreAuthenticate = true;

    request.AddUrlSegment("projectId", projectId.ToString());
    request.AddUrlSegment("idOrOutlinenumber", idOrOutlinenumber);

    request.AddQueryParameter("insertMode", "insertBefore");
    request.AddQueryParameter("idOrOutlinenumberType", "outlineNumber");
    request.AddXmlBody(xmlWBS);

    // レスポンスを取得する
    var result = client.Execute(request);
    string ret = string.Empty;
    ret = result.Content;

    return ret;
}

#endregion WBSの一括作成

#region WBSの一括更新

/// <summary>
/// WBSを一括更新する
/// </summary>
private void BatchUpdateWBSData()
{
    //  WBS一括更新用のURL
    //  - ID 4のプロジェクトのアウトライン番号1を指定
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    int projectId = 4;
    string outlineNumber = "1";

    XElement links;
    int linkfromId = 0;

    // WBSデータを取得する
    string resource = "/projects/{projectId}/wbsnodes/{idOrOutlinenumber}";
    string data =  GetProjectWBSData(url, resource, projectId, outlineNumber);
    XElement xmlWBS = XElement.Parse(data);

    foreach (XElement element in xmlWBS.Descendants("wbsnode"))
    {
        // アウトライン番号を取得
        XElement node = element.Element("outlineNumber");

        if (node.Value == "1.1.1")
        {
            // 「基本設計」タスクの名前を「機能1基本設計」に変更
            XElement name = element.Element("name");
            name.Value = "機能1基本設計";

        }
        else if (node.Value == "1.1.2")
        {
            // 「基本設計レビュー」タスクを削除
            element.Add(new XElement("deleted", "True"));
        }
        else if (node.Value == "1.2.1")
        {
            //「詳細設計」タスクのリソース割り当てを削除
            foreach (XElement assignment in element.Descendants("assignment"))
            {
                XElement deleted = assignment.Element("deleted");
                deleted.Value = "True";
            }

            // リンク元ノードとするため記憶する
            linkfromId = Convert.ToInt32(element.Element("id").Value);

        }
        else if (node.Value == "1.2.2")
        {
            // リンク作成　「詳細設計」タスクをリンク元ノードとする
            links = new XElement("links",
                        new XElement("link",
                            new XElement("fromId", linkfromId)
                            )
                    );

            element.Add(links);
           
        }
    }
    
    // 一括更新を行う
    resource = "/projects/{projectId}/wbsnodes/massUpdate/{idOrOutlinenumber}";
    BatchUpdateWBSData(url, resource, projectId, "-1", xmlWBS);

    // 出力するファイル名
    string fileName = @"C:\Users\sampleuser\Documents\ProjectWBSList.xml";

    // 取得したWBSデータをファイルに出力する
    Encoding utf8 = Encoding.GetEncoding("UTF-8");
    StreamWriter sw = new StreamWriter(fileName, false, utf8);
    sw.Write(data);
    sw.Close();
}

/// <summary>
/// WBSデータを一括更新する
/// </summary>
/// <param name="url">API実行用URL</param>
/// <param name="resource">API実行用リソース</param>
/// <param name="projectId">対象のプロジェクト</param>
/// <param name="idOrOutlinenumber">基準となるノードの位置</param>
/// <param name="xmlWBS"></param>
/// <returns>プロジェクトデータ(XML)</returns>
private string BatchUpdateWBSData(string url, string resource, int projectId, string idOrOutlinenumber, XElement xmlWBS)
{
    // 接続に使用するログイン名とパスワードを指定する
    string user = "okamoto";
    string password = string.Empty; // 空文字

    // リクエストを作成する
    var client = new RestClient(url);
    var request = new RestRequest(resource, Method.PUT);
    request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
    request.Credentials = new NetworkCredential(user, password);
    request.AddHeader("Content-Type", "application/xml");
    client.PreAuthenticate = true;

    request.AddUrlSegment("projectId", projectId.ToString());
    request.AddUrlSegment("idOrOutlinenumber", idOrOutlinenumber.ToString());
    
    request.AddXmlBody(xmlWBS);

    // レスポンスを取得する
    var result = client.Execute(request);
    string ret = string.Empty;
    ret = result.Content;

    return ret;
}

/// <summary>
/// WBSデータを取得する(取得位置指定、リソース割り当て情報あり)
/// </summary>
/// <param name="url">API実行用URL</param>
/// <param name="resource">API実行用リソース</param>
/// <param name="projectId">対象のプロジェクト</param>
/// <param name="projectId">対象のノードまたはアウトライン番号</param>
/// <returns>プロジェクトデータ(XML)</returns>
private string GetProjectWBSData(string url, string resource, int projectId, string idOrOutlinenumber)
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

    request.AddUrlSegment("projectId", projectId.ToString());
    request.AddUrlSegment("idOrOutlinenumber", idOrOutlinenumber.ToString());

    request.AddQueryParameter("fields", "assignments");
    request.AddQueryParameter("idOrOutlinenumberType", "outlineNumber");

    // レスポンスを取得する
    var result = client.Execute(request);
    string ret = string.Empty;
    ret = result.Content;

    return ret;
}

#endregion WBSの一括更新