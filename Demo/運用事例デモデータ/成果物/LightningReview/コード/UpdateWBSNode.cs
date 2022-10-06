#region WBS�̈ꊇ�쐬

/// <summary>
/// WBS���ꊇ�쐬����
/// </summary>
private void BatchAddWBSData()
{
    //  WBS�ꊇ�쐬�p��URL
    //  - ID 4�̃v���W�F�N�g�̃A�E�g���C���ԍ�1���w��
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/projects/{projectId}/wbsnodes/massUpdate/{idOrOutlinenumber}";
    int projectId = 4;
    string outlineNumber = "1";

    // �ꊇ�쐬����WBS���`����
    //   WBS�̍\���͈ȉ��̒ʂ�
    //   �e�^�X�N�ɁA�A�J�E���gID 3, 4, 6�����蓖�Ă�
    //   + �݌v
    //     + ��{�݌v
    //      -- ��{�݌v
    //      -- ��{�݌v���r���[
    //     + �ڍא݌v
    //      -- �ڍא݌v
    //      -- �ڍא݌v���r���[

    #region WBS�̒�`

    XElement xmlWBS = new XElement("wbsnodes",
        new XElement("wbsnode",
            new XElement("name", "�݌v"),
            new XElement("kind", "taskPackage"),
            new XElement("children",
                new XElement("wbsnode",
                    new XElement("name", "��{�݌v"),
                    new XElement("kind", "taskPackage"),
                    new XElement("children",
                        new XElement("wbsnode",
                            new XElement("name", "��{�݌v"),
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
                            new XElement("name", "��{�݌v���r���["),
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
                    new XElement("name", "�ڍא݌v"),
                    new XElement("kind", "taskPackage"),
                    new XElement("children",
                        new XElement("wbsnode",
                            new XElement("name", "�ڍא݌v"),
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
                            new XElement("name", "�ڍא݌v���r���["),
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

    #endregion WBS�̒�`

    // WBS�f�[�^���ꊇ�쐬����
    BatchAddWBSData(url, resource, projectId, outlineNumber, xmlWBS);

    // WBS�f�[�^���擾����
    resource = "/projects/{id}/wbsnodes";
    string data = GetProjectWBSData(url, resource, projectId);

    // �o�͂���t�@�C����
    string fileName = @"C:\Users\sampleuser\Documents\ProjectWBSList.xml";

    // �擾����WBS�f�[�^���t�@�C���ɏo�͂���
    Encoding utf8 = Encoding.GetEncoding("UTF-8");
    StreamWriter sw = new StreamWriter(fileName, false, utf8);
    sw.Write(data);
    sw.Close();
}

/// <summary>
/// WBS�f�[�^���ꊇ�쐬����
/// </summary>
/// <param name="url">API���s�pURL</param>
/// <param name="resource">API���s�p���\�[�X</param>
/// <param name="projectId">�Ώۂ̃v���W�F�N�g</param>
/// <param name="idOrOutlinenumber">��ƂȂ�m�[�h�̈ʒu</param>
/// <param name="xmlWBS">�m�[�h�f�[�^</param>
/// <returns>�v���W�F�N�g�f�[�^(XML)</returns>
private string BatchAddWBSData(string url, string resource, int projectId, string idOrOutlinenumber, XElement xmlWBS)
{
    // �ڑ��Ɏg�p���郍�O�C�����ƃp�X���[�h���w�肷��
    string user = "okamoto";
    string password = string.Empty; // �󕶎�

    // ���N�G�X�g���쐬����
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

    // ���X�|���X���擾����
    var result = client.Execute(request);
    string ret = string.Empty;
    ret = result.Content;

    return ret;
}

#endregion WBS�̈ꊇ�쐬

#region WBS�̈ꊇ�X�V

/// <summary>
/// WBS���ꊇ�X�V����
/// </summary>
private void BatchUpdateWBSData()
{
    //  WBS�ꊇ�X�V�p��URL
    //  - ID 4�̃v���W�F�N�g�̃A�E�g���C���ԍ�1���w��
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    int projectId = 4;
    string outlineNumber = "1";

    XElement links;
    int linkfromId = 0;

    // WBS�f�[�^���擾����
    string resource = "/projects/{projectId}/wbsnodes/{idOrOutlinenumber}";
    string data =  GetProjectWBSData(url, resource, projectId, outlineNumber);
    XElement xmlWBS = XElement.Parse(data);

    foreach (XElement element in xmlWBS.Descendants("wbsnode"))
    {
        // �A�E�g���C���ԍ����擾
        XElement node = element.Element("outlineNumber");

        if (node.Value == "1.1.1")
        {
            // �u��{�݌v�v�^�X�N�̖��O���u�@�\1��{�݌v�v�ɕύX
            XElement name = element.Element("name");
            name.Value = "�@�\1��{�݌v";

        }
        else if (node.Value == "1.1.2")
        {
            // �u��{�݌v���r���[�v�^�X�N���폜
            element.Add(new XElement("deleted", "True"));
        }
        else if (node.Value == "1.2.1")
        {
            //�u�ڍא݌v�v�^�X�N�̃��\�[�X���蓖�Ă��폜
            foreach (XElement assignment in element.Descendants("assignment"))
            {
                XElement deleted = assignment.Element("deleted");
                deleted.Value = "True";
            }

            // �����N���m�[�h�Ƃ��邽�ߋL������
            linkfromId = Convert.ToInt32(element.Element("id").Value);

        }
        else if (node.Value == "1.2.2")
        {
            // �����N�쐬�@�u�ڍא݌v�v�^�X�N�������N���m�[�h�Ƃ���
            links = new XElement("links",
                        new XElement("link",
                            new XElement("fromId", linkfromId)
                            )
                    );

            element.Add(links);
           
        }
    }
    
    // �ꊇ�X�V���s��
    resource = "/projects/{projectId}/wbsnodes/massUpdate/{idOrOutlinenumber}";
    BatchUpdateWBSData(url, resource, projectId, "-1", xmlWBS);

    // �o�͂���t�@�C����
    string fileName = @"C:\Users\sampleuser\Documents\ProjectWBSList.xml";

    // �擾����WBS�f�[�^���t�@�C���ɏo�͂���
    Encoding utf8 = Encoding.GetEncoding("UTF-8");
    StreamWriter sw = new StreamWriter(fileName, false, utf8);
    sw.Write(data);
    sw.Close();
}

/// <summary>
/// WBS�f�[�^���ꊇ�X�V����
/// </summary>
/// <param name="url">API���s�pURL</param>
/// <param name="resource">API���s�p���\�[�X</param>
/// <param name="projectId">�Ώۂ̃v���W�F�N�g</param>
/// <param name="idOrOutlinenumber">��ƂȂ�m�[�h�̈ʒu</param>
/// <param name="xmlWBS"></param>
/// <returns>�v���W�F�N�g�f�[�^(XML)</returns>
private string BatchUpdateWBSData(string url, string resource, int projectId, string idOrOutlinenumber, XElement xmlWBS)
{
    // �ڑ��Ɏg�p���郍�O�C�����ƃp�X���[�h���w�肷��
    string user = "okamoto";
    string password = string.Empty; // �󕶎�

    // ���N�G�X�g���쐬����
    var client = new RestClient(url);
    var request = new RestRequest(resource, Method.PUT);
    request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
    request.Credentials = new NetworkCredential(user, password);
    request.AddHeader("Content-Type", "application/xml");
    client.PreAuthenticate = true;

    request.AddUrlSegment("projectId", projectId.ToString());
    request.AddUrlSegment("idOrOutlinenumber", idOrOutlinenumber.ToString());
    
    request.AddXmlBody(xmlWBS);

    // ���X�|���X���擾����
    var result = client.Execute(request);
    string ret = string.Empty;
    ret = result.Content;

    return ret;
}

/// <summary>
/// WBS�f�[�^���擾����(�擾�ʒu�w��A���\�[�X���蓖�ď�񂠂�)
/// </summary>
/// <param name="url">API���s�pURL</param>
/// <param name="resource">API���s�p���\�[�X</param>
/// <param name="projectId">�Ώۂ̃v���W�F�N�g</param>
/// <param name="projectId">�Ώۂ̃m�[�h�܂��̓A�E�g���C���ԍ�</param>
/// <returns>�v���W�F�N�g�f�[�^(XML)</returns>
private string GetProjectWBSData(string url, string resource, int projectId, string idOrOutlinenumber)
{
    // �ڑ��Ɏg�p���郍�O�C�����ƃp�X���[�h���w�肷��
    string user = "okamoto";
    string password = string.Empty; // �󕶎�

    // ���N�G�X�g���쐬����
    var client = new RestClient(url);
    var request = new RestRequest(resource, Method.GET);
    request.Credentials = new NetworkCredential(user, password);
    request.AddHeader("Content-Type", "application/xml");
    client.PreAuthenticate = true;

    request.AddUrlSegment("projectId", projectId.ToString());
    request.AddUrlSegment("idOrOutlinenumber", idOrOutlinenumber.ToString());

    request.AddQueryParameter("fields", "assignments");
    request.AddQueryParameter("idOrOutlinenumberType", "outlineNumber");

    // ���X�|���X���擾����
    var result = client.Execute(request);
    string ret = string.Empty;
    ret = result.Content;

    return ret;
}

#endregion WBS�̈ꊇ�X�V