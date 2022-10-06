#region �v���W�F�N�g�̎擾

/// <summary>
/// �v���W�F�N�g���擾����
/// </summary>
private void GetProjectData()
{
    // �v���W�F�N�g�f�[�^�擾(GET)�p��URL
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/projects";

    // �v���W�F�N�g�f�[�^���擾����
    string data = GetProjectData(url, resource);

    // �o�͂���t�@�C����
    string fileName = @"C:\Users\sampleuser\Documents\ProjectList.xml";

    // �擾�����v���W�F�N�g�f�[�^���t�@�C���ɏo�͂���
    Encoding utf8 = Encoding.GetEncoding("UTF-8");
    StreamWriter sw = new StreamWriter(fileName, false, utf8);
    sw.Write(data);
    sw.Close();
}


/// <summary>
/// �v���W�F�N�g�f�[�^���擾����
/// </summary>
/// <param name="url">API���s�pURL</param>
/// <param name="resource">API���s�p���\�[�X</param>
/// <returns>�v���W�F�N�g�f�[�^(XML)</returns>
private string GetProjectData(string url, string resource)
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
    
    // ���X�|���X���擾����
    var result = client.Execute(request);
    string ret = string.Empty;
    ret = result.Content;

    return ret;
}

#endregion �v���W�F�N�g�̎擾

#region WBS�̎擾

/// <summary>
/// WBS���擾����
/// </summary>
private void GetProjectWBSData()
{
    // �v���W�F�N�g�f�[�^�擾(GET)�p��URL
    //  - ID 4�̃v���W�F�N�g���w��
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/projects/{id}/wbsnodes";
    int projectId = 4;

    // WBS�f�[�^���擾����
    string data = GetProjectWBSData(url, resource, projectId);

    // �o�͂���t�@�C����
    string fileName = @"C:\Users\sampleuser\Documents\ProjectWBSList.xml";

    // �擾�����v���W�F�N�g�f�[�^���t�@�C���ɏo�͂���
    Encoding utf8 = Encoding.GetEncoding("UTF-8");
    StreamWriter sw = new StreamWriter(fileName, false, utf8);
    sw.Write(data);
    sw.Close();
}


/// <summary>
/// WBS�f�[�^���擾����
/// </summary>
/// <param name="url">API���s�pURL</param>
/// <param name="resource">API���s�p���\�[�X</param>
/// <param name="projectId">�Ώۂ̃v���W�F�N�g</param>
/// <returns>�v���W�F�N�g�f�[�^(XML)</returns>
private string GetProjectWBSData(string url, string resource, int projectId)
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

    request.AddUrlSegment("id", projectId.ToString());

    // ���X�|���X���擾����
    var result = client.Execute(request);
    string ret = string.Empty;
    ret = result.Content;

    return ret;
}

#endregion WBS�̎擾

#region �v���W�F�N�g�̍쐬

/// <summary>
/// �v���W�F�N�g��V�K�ɍ쐬����
/// </summary>
private void AddProjectData()
{
    // �v���W�F�N�g�f�[�^�ǉ�(POST)�p��URL
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc/";
    string resource = "/projects";

    // �ǉ�����v���W�F�N�g�̃f�[�^���i�[����CSV�t�@�C�����w�肷��
    string csvProject = @"C:\Users\sampleuser\Documents\Projects.csv";

    // �e�v���W�F�N�g�ɓo�^���郁���o�[�̃A�J�E���gID���w�肷��
    int[] members = new int[] { 4, 30, 31 };

    // �v���W�F�N�gWBS�i�W��WBS�j���`����
    //   WBS�̍\���͈ȉ��̒ʂ�
    //     + �O���d�l
    //      -- �O���d�l���쐬
    //      -- �O���d�l���r���[
    XElement xmlWBS = new XElement("wbsnodes",
        new XElement("wbsnode",
            new XElement("name", "�O���d�l"),
            new XElement("code", "SPEC"),
            new XElement("kind", "taskPackage"),
            new XElement("children",
                new XElement("wbsnode",
                    new XElement("name", "�O���d�l���쐬"),
                    new XElement("code", "SPEC-01"),
                    new XElement("kind", "task")
                ),
                new XElement("wbsnode",
                    new XElement("name", "�O���d�l���r���["),
                    new XElement("code", "SPEC-02"),
                    new XElement("kind", "task")
                )
            )
        )
    );

    // �v���W�F�N�g���쐬����
    ArrayList ret = AddProjectData(url, resource, csvProject, members, xmlWBS);

    // �������ʁi�쐬�����v���W�F�N�g��ID�j����ʂɏo�͂���
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

    // �ilabelResult�͉�ʏ�ɐݒ肳�ꂽ�I�u�W�F�N�g�j
    labelResult.Text = "�v���W�F�N�g�̍쐬���������܂����BID = " + displayText;
}

/// <summary>
/// �v���W�F�N�g��V�K�ɍ쐬����
/// </summary>
/// <param name="url">API���s�pURL�i�v���W�F�N�g�쐬�j</param>
/// <param name="resource">API���s�p���\�[�X�i�v���W�F�N�g�쐬�j</param>
/// <param name="csvProject">�쐬����v���W�F�N�g�f�[�^(CSV�t�@�C��)</param>
/// <param name="members">�e�v���W�F�N�g�ɓo�^���郁���o�[�i�A�J�E���gID�j</param>
/// <param name="xmlWBS">�e�v���W�F�N�g�ɓo�^����WBS(XML)</param>
/// <returns>�쐬�����v���W�F�N�g��ID</returns>
private ArrayList AddProjectData(string url, string resource, string csvProject, int[] members, XElement xmlWBS)
{
    // ���X�|���X�i�ǉ�����ID�̃��X�g�j���i�[���邽�߂̔z��
    ArrayList IdList = new ArrayList();

    // �v���W�F�N�g��񂪊i�[���ꂽCSV�t�@�C����ǂݍ���
    StreamReader csvsr = new StreamReader(csvProject, Encoding.GetEncoding("Shift_JIS"));

    // 1�s���f�[�^��ǉ�����
    while (csvsr.EndOfStream == false)
    {
        string line = csvsr.ReadLine();
        string[] fields = line.Split(',');

        // ==========================
        // ====== �v���W�F�N�g ======
        // ==========================

        // ���M�p��XML�f�[�^���쐬����
        // �v���W�F�N�g��CSV�t�@�C���ɂ͈ȉ��̏����Œ�`����Ă�����̂Ƃ���
        //   �v���W�F�N�g���A�v���W�F�N�g�R�[�h�A�}�l�[�W��ID�A�v��J�n���A�v��I����
        XElement xml = new XElement("project",
            new XElement("name", fields[0]),
            new XElement("code", fields[1]),
            new XElement("managerId", fields[2]),
            new XElement("plannedStartDate", fields[3]),
            new XElement("plannedFinishDate", fields[4])
        );

        // �v���W�F�N�g���쐬����
        int id = ExecuteAddProjectRequest(url, resource, -1, xml, Method.POST);

        IdList.Add(id.ToString()); // �ǉ�����ID�����X�g�ɒǉ����Ă���

        // ======================
        // ====== �����o�[ ======
        // ======================

        // �e�v���W�F�N�g�ɋ��ʂ̃����o�[��o�^����
        foreach (int accountId in members)
        {
            // �e�A�J�E���g��XML�f�[�^���쐬����
            xml = new XElement("member",
                    new XElement("accountId", accountId.ToString())
            );

            // �����o�[�ǉ�(POST)�p��URL
            string resourceMember = "/projects/{id}/members";

            // �����o�[��o�^����
            ExecuteAddProjectRequest(url, resourceMember, id, xml, Method.POST);
        }

        // =================
        // ====== WBS ======
        // =================

        // WBS�ǉ�(PUT)�p��URL�i���[�g�m�[�h�����ɒǉ�����j
        string resourceWBS = "/projects/{id}/wbsnodes/massUpdate/-1?insertMode=children";

        // WBS��o�^����
        ExecuteAddProjectRequest(url, resourceWBS, id, xmlWBS, Method.PUT);
    }

    return IdList;
}

/// <summary>
/// �v���W�F�N�g�V�K�쐬�̃��N�G�X�g�����s����
/// </summary>
/// <param name="url">API���s�pURL</param>
/// <param name="resource">API���s�p���\�[�X</param>
/// <param name="projectId">�Ώۂ̃v���W�F�N�g(�v���W�F�N�g�쐬���� -1)</param>
/// <param name="xml">�쐬����f�[�^(XML)</param>
/// <param name="method">���s����Web API�̃��\�b�h(POST, PUT)</param>
/// <returns>�ǉ������I�u�W�F�N�g��ID</returns>
private int ExecuteAddProjectRequest(string url, string resource, int projectId, XElement xml, Method method)
{
    // �ڑ��Ɏg�p���郍�O�C�����ƃp�X���[�h���w�肷��
    string user = "okamoto";
    string password = string.Empty; // �󕶎�
    
    // ���N�G�X�g���쐬����
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

    // ���X�|���X���擾����
    var result = client.Execute(request);

    // �ǉ�����ID����߂�l�Ƃ���iPOST�̏ꍇ�̂݁j
    int ret = 0;
    if (method == Method.POST)
    {
        ret = Convert.ToInt32(result.Content.Trim('"'));
    }

    return ret;
}

#endregion �v���W�F�N�g�̍쐬