#region �A�J�E���g�̎擾

/// <summary>
/// �A�J�E���g���擾����
/// </summary>
private void GetAccountData()
{
    // �A�J�E���g�擾(GET)�p��URL
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/system/accounts";
    
    // �A�J�E���g�����擾����
    string data = GetAccountData(url, resource);

    // �o�͂���t�@�C����
    string fileName = @"C:\Users\sampleuser\Documents\AccountList.xml";

    // �擾�����A�J�E���g�����t�@�C���ɏo�͂���
    Encoding utf8 = Encoding.GetEncoding("UTF-8");
    StreamWriter sw = new StreamWriter(fileName, false, utf8);
    sw.Write(data);
    sw.Close();
}

/// <summary>
/// �A�J�E���g���擾����
/// </summary>
/// <param name="url">API���s�pURL</param>
/// <param name="resource">API���s�p���\�[�X</param>
/// <returns>�A�J�E���g�f�[�^</returns>
private string GetAccountData(string url, string resource)
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

    var result = client.Execute(request);

    // ���X�|���X���擾����
    string ret = string.Empty;
    ret = result.Content;

    return ret;
}

#endregion �A�J�E���g�̎擾

#region �A�J�E���g�̍X�V

/// <summary>
/// �A�J�E���g���X�V����
/// </summary>
private void UpdateAccountData()
{
    // �A�J�E���g�X�V(PUT)�p��URL
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/system/accounts/{id}";

    // �X�V���e���`����
    // �i�g�D�E�����E�R�X�g�P����ύX�j
    int accountId = 12; // �X�V�Ώۂ̃A�J�E���gID
    XElement xml = new XElement("account",
        new XElement("name", "�A�c �M�M"),
        new XElement("code", "009"),
        new XElement("loginName", "ueda"),
        new XElement("sectionId", 4),
        new XElement("roleId", 3),
        new XElement("unitCost", 8000)
    );

    // �A�J�E���g�̃f�[�^���X�V����
    string ret = UpdateAccountData(url, resource, accountId, xml);

    // �������ʂ���ʂɏo�͂���
    // �ilabelResult�͉�ʏ�ɐݒ肳�ꂽ�I�u�W�F�N�g�j
    labelResult.Text = "�A�J�E���g�̍X�V���������܂����B";

    // �X�V��̃A�J�E���g�ꗗ�����擾����
    GetAccountData();
}

/// <summary>
/// �A�J�E���g���X�V����
/// </summary>
/// <param name="url">API���s�pURL</param>
/// <param name="resource">API���s�p���\�[�X</param>
/// <param name="accountId">�Ώۂ̃A�J�E���g</param>
/// <param name="xml">�X�V�f�[�^�iXML�`���j</param>
/// <returns></returns>
private string UpdateAccountData(string url, string resource, int accountId, XElement xml)
{
    // �ڑ��Ɏg�p���郍�O�C�����ƃp�X���[�h���w�肷��
    string user = "okamoto";
    string password = string.Empty; // �󕶎�

    // XML�f�[�^�𑗐M�\�Ȍ^�ɕϊ�����
    byte[] data = Encoding.UTF8.GetBytes(xml.ToString());

    // ���N�G�X�g���쐬����i�A�J�E���g�f�[�^���X�V�j
    var client = new RestClient(url);
    var request = new RestRequest(resource, Method.PUT);
    client.PreAuthenticate = true;
    request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
    request.AddHeader("Content-Type", "application/xml");
    request.Credentials = new NetworkCredential(user, password);

    request.AddUrlSegment("id", accountId.ToString());
    request.AddXmlBody(xml);

    var result = client.Execute(request);

    // ���X�|���X���擾����
    string ret = result.ErrorMessage;

    // ����ɏI�������ꍇ�͋󕶎����Ԃ�
    return ret;
}

#endregion �A�J�E���g�̍X�V

#region �A�J�E���g�̒ǉ�

/// <summary>
/// �A�J�E���g��ǉ�����
/// </summary>
private void AddAccountData()
{
    // �A�J�E���g�ǉ�(POST)�p��URL
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/system/accounts";

    // �ǉ�����A�J�E���g�̃f�[�^���i�[����CSV�t�@�C�����w�肷��
    string csvFile = @"C:\Users\sampleuser\Documents\accounts.csv";

    // �A�J�E���g��ǉ�����
    ArrayList ret = AddAccountData(url, resource, csvFile);

    // �������ʁi�ǉ������A�J�E���g��ID�j����ʂɏo�͂���
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
    labelResult.Text = "�A�J�E���g�̒ǉ����������܂����B" + displayText;

    // �ǉ���̃A�J�E���g�ꗗ�����擾����
    GetAccountData();
}

/// <summary>
/// �A�J�E���g��V�K�ɒǉ�����
/// </summary>
/// <param name="url">API���s�pURL</param>
/// <param name="resource">API���s�p���\�[�X</param>
/// <param name="csvFile">�ǉ��A�J�E���g�̃f�[�^���i�[���ꂽCSV�t�@�C����</param>
/// <returns>�ǉ������A�J�E���gID�̃��X�g</returns>
private ArrayList AddAccountData(string url, string resource, string csvFile)
{
    // �ڑ��Ɏg�p���郍�O�C�����ƃp�X���[�h���w�肷��
    string user = "okamoto";
    string password = string.Empty; // �󕶎�

    // �A�J�E���g��񂪊i�[���ꂽCSV�t�@�C����ǂݍ���
    StreamReader csvsr = new StreamReader(csvFile, Encoding.GetEncoding("Shift_JIS"));

    // ���X�|���X�i�ǉ�����ID�̃��X�g�j���i�[���邽�߂̔z��
    ArrayList idList = new ArrayList();

    // 1�s���f�[�^��ǉ�����
    while(csvsr.EndOfStream == false)
    {
        string line = csvsr.ReadLine();
        string[] fields = line.Split(',');

        // �e�t�B�[���h�̒l���擾����
        string name = fields[0];    // �A�J�E���g��
        string code = fields[1];    // �A�J�E���g�R�[�h
        string loginName = fields[2];   // ���O�C����
        int sectionId = Convert.ToInt32(fields[3]); // �g�DID
        int roleId = Convert.ToInt32(fields[4]);    // ����ID
        int unitCost = Convert.ToInt32(fields[5]);  // �R�X�g�P��

        // ���M�p��XML�f�[�^���쐬����
        XElement xml = new XElement("account",
            new XElement("name", name),
            new XElement("code", code),
            new XElement("loginName", loginName),
            new XElement("sectionId", sectionId),
            new XElement("roleId", roleId),
            new XElement("unitCost", unitCost)                    
        );
        
        // ���N�G�X�g���쐬����
        var client = new RestClient(url);
        var request = new RestRequest(resource, Method.POST);
        client.PreAuthenticate = true;
        request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
        request.Credentials = new NetworkCredential(user, password);
        request.AddHeader("Content-Type", "application/xml");

        request.AddXmlBody(xml);

        var result = client.Execute(request);
        
        // ���X�|���X���擾����
        idList.Add(result.Content); // �ǉ�����ID�����X�g�ɒǉ����Ă���
    }

    return idList;
}

#endregion  �A�J�E���g�̒ǉ�