#region ���эH���擾

/// <summary>
/// ���эH���f�[�^���擾����i���ђP�ʁj
/// </summary>
private void GetActualTime()
{
    // ���эH���f�[�^�擾�p��URL�iID10�̃A�J�E���g���w��j
    //   �w�肵�����Ԃ̃f�[�^�����ђP�ʂŎ擾����
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/accounts/{id}/ActualTimes";
    int account = 10;
    string start = "2016-02-01";    // �擾�Ώۊ��ԁi�J�n���j
    string finish = "2016-02-01";   // �擾�Ώۊ��ԁi�I�����j

    // ���эH���f�[�^���擾����
    string data = GetActualTime(url, resource, account, start, finish);

    // �o�͂���t�@�C����
    string fileName = @"C:\Users\sampleuser\Documents\ActualTime.xml";

    // �擾�������эH���f�[�^���t�@�C���ɏo�͂���
    Encoding utf8 = Encoding.GetEncoding("UTF-8");
    StreamWriter sw = new StreamWriter(fileName, false, utf8);
    sw.Write(data);
    sw.Close();
}

/// <summary>
/// ���эH���f�[�^���擾����i���P�ʁj
/// </summary>
private void GetDailyActualTime()
{
    // ���эH���f�[�^�擾�p��URL�iID10�̃A�J�E���g���w��j
    //   �w�肵�����Ԃ̃f�[�^����P�ʂŎ擾����
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc";
    string resource = "/accounts/{id}/dailyActualTimes";
    int account = 10;
    string start = "2016-02-01";    // �擾�Ώۊ��ԁi�J�n���j
    string finish = "2016-02-01";   // �擾�Ώۊ��ԁi�I�����j

    // ���эH���f�[�^���擾����
    string data = GetActualTime(url, resource, account, start, finish);

    // �o�͂���t�@�C����
    string fileName = @"C:\Users\sampleuser\Documents\DailyActualTime.xml";

    // �擾�������эH���f�[�^���t�@�C���ɏo�͂���
    Encoding utf8 = Encoding.GetEncoding("UTF-8");
    StreamWriter sw = new StreamWriter(fileName, false, utf8);
    sw.Write(data);
    sw.Close();
}

/// <summary>
/// ���эH���f�[�^���擾����
/// </summary>
/// <param name="url">API���s�pURL</param>
/// <param name="resource">API���s�p���\�[�X</param>
/// <param name="accountId">�Ώۂ̃A�J�E���g</param>
/// <param name="start">�p�����[�^�F�J�n��</param>
/// <param name="finish">�p�����[�^�F�I����</param>
/// <returns>���эH���f�[�^</returns>
private string GetActualTime(string url, string resource, int accountId, string start, string finish)
{
    // �ڑ��Ɏg�p���郍�O�C�����ƃp�X���[�h���w�肷��
    string user = "okamoto";
    string password = string.Empty;

    // ���N�G�X�g���쐬����
    var client = new RestClient(url);
    var request = new RestRequest(resource, Method.GET);
    request.Credentials = new NetworkCredential(user, password);
    request.AddHeader("Content-Type", "application/xml");
    client.PreAuthenticate = true;

    // �e���v���[�g�p�����[�^�A�N�G���p�����[�^��ǉ�����
    request.AddUrlSegment("id", accountId.ToString());
    request.AddQueryParameter("startDate", start);
    request.AddQueryParameter("finishDate", finish);

    var result = client.Execute(request);

    // ���X�|���X���擾����
    string ret = string.Empty;
    ret = result.Content;
    
    return ret;
}

#endregion ���эH���擾

#region ���эH���ǉ�

/// <summary>
/// ���эH���f�[�^��ǉ�����
/// </summary>
private void AddActualTime()
{
    // ���эH���f�[�^�ǉ�(POST)�p��URL�iID10�̃A�J�E���g���w��j
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc/";
    string resource = "/accounts/{id}/actualTimes";
    int accountId = 10;

    // �ΏۃA�J�E���g�ɂ��Ēǉ�����H���f�[�^���i�[����CSV�t�@�C�����w�肷��
    string csvFile = @"C:\Users\sampleuser\Documents\ActualTime.csv";

    // ���эH���f�[�^��ǉ�����
    ArrayList ret = AddActualTime(url, resource, accountId, csvFile);

    // �������ʁi�ǉ��������эH���f�[�^�j����ʂɏo�͂���
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
    labelResult.Text = "���уf�[�^�̒ǉ����������܂����B" + displayText;

}

/// <summary>
/// ���эH���f�[�^��V�K�ɒǉ�����
/// </summary>
/// <param name="url">API���s�pURL</param>
/// <param name="resource">API���s�p���\�[�X</param>
/// <param name="accountId">�Ώۂ̃A�J�E���g</param>
/// <param name="csvFile">���эH���f�[�^���i�[���ꂽCSV�t�@�C����</param>
/// <returns>�ǉ���������ID�̃��X�g</returns>
private ArrayList AddActualTime(string url, string resource, int accountId, string csvFile)
{
    // �ڑ��Ɏg�p���郍�O�C�����ƃp�X���[�h���w�肷��
    string user = "okamoto";
    string password = string.Empty;     // �󕶎��i�T���v���f�[�^�̊���l�j

    // ���я�񂪊i�[���ꂽCSV�t�@�C����ǂݍ���
    StreamReader csvsr = new StreamReader(csvFile, Encoding.GetEncoding("Shift_JIS"));

    // ���X�|���X�i�ǉ�����ID�̃��X�g�j���i�[���邽�߂̔z��
    ArrayList idList = new ArrayList();

    // 1�s���f�[�^��ǉ�����
    while (csvsr.EndOfStream == false)
    {
        string line = csvsr.ReadLine();
        string[] fields = line.Split(',');

        // �e�t�B�[���h�̒l���擾����
        int projectId = Convert.ToInt32(fields[0]);     // �v���W�F�N�gID
        int taskId = Convert.ToInt32(fields[1]);        // �^�X�NID
        string startTime = fields[2];                   // �J�n����
        string finishTime = fields[3];                  // �I������
        string memo = fields[4];                        // ����

        // ���M�p��XML�f�[�^���쐬����
        XElement xml = new XElement("actualTime",
            new XElement("projectId", projectId),
            new XElement("taskId", taskId),
            new XElement("startTime", startTime),
            new XElement("finishTime", finishTime),
            new XElement("memo", memo)
        );
        
        // ���N�G�X�g���쐬����
        var client = new RestClient(url);
        var request = new RestRequest(resource, Method.POST);
        client.PreAuthenticate = true;
        request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
        request.Credentials = new NetworkCredential(user, password);
        request.AddHeader("Content-Type", "application/xml");

        request.AddUrlSegment("id", accountId.ToString());
        request.AddXmlBody(xml);

        // ���X�|���X���擾����
        var result = client.Execute(request);
        idList.Add(result.Content); // �ǉ�����ID�����X�g�ɒǉ����Ă���
    }
    return idList;
}

#endregion ���эH���ǉ�

#region ���эH���ǉ��i�i�������܂ށj

/// <summary>
/// ���эH���f�[�^��ǉ�����i�i�������܂ށj
/// </summary>
private void AddActualTimeProgress()
{
    // ���эH���f�[�^�ǉ�(POST)�p��URL�iID10�̃A�J�E���g���w��j
    string url = "http://WebServer/TimeTrackerFX/xmlWebService.svc/";
    string resource = "/accounts/{id}/actualTimes";
    int accountId = 10;
    
    // �ΏۃA�J�E���g�ɂ��Ēǉ�����H���f�[�^���i�[����CSV�t�@�C�����w�肷��
    string csvFile = @"C:\Users\sampleuser\Documents\actualtime_progress.csv";

    // ���эH���f�[�^��ǉ�����i�i�������܂ށj
    ArrayList ret = AddActualTimeProgress(url, resource, accountId, csvFile);

    // �������ʁi�ǉ��������эH���f�[�^�j����ʂɏo�͂���
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
    labelResult.Text = "���уf�[�^�̒ǉ����������܂����B" + displayText;
}

/// <summary>
/// ���эH���f�[�^��V�K�ɒǉ�����i�i�������܂ށj
/// </summary>
/// <param name="url">API���s�pURL�i���эH���ǉ��p�j</param>
/// <param name="resource">API���s�p���\�[�X�i���蓖�ă^�X�N���X�V�p�j</param>
/// <param name="accountId">�Ώۂ̃A�J�E���g</param>
/// <param name="csvFile">�i�������܂ގ��эH���f�[�^���i�[���ꂽCSV�t�@�C����</param>
/// <returns>�ǉ���������ID�̃��X�g</returns>
private ArrayList AddActualTimeProgress(string url, string resource, int accountId, string csvFile)
{
    // �ڑ��Ɏg�p���郍�O�C�����ƃp�X���[�h���w�肷��
    string user = "okamoto";
    string password = string.Empty;     // �󕶎��i�T���v���f�[�^�̊���l�j

    // ���я�񂪊i�[���ꂽCSV�t�@�C����ǂݍ���
    StreamReader csvsr = new StreamReader(csvFile, Encoding.GetEncoding("Shift_JIS"));

    // ���X�|���X�i�ǉ�����ID�̃��X�g�j���i�[���邽�߂̔z��
    ArrayList idList = new ArrayList();

    // 1�s���f�[�^��ǉ�����
    while (csvsr.EndOfStream == false)
    {
        string line = csvsr.ReadLine();
        string[] fields = line.Split(',');

        // �e�t�B�[���h�̒l���擾����
        int projectId = Convert.ToInt32(fields[0]);     // �v���W�F�N�gID
        int taskId = Convert.ToInt32(fields[1]);        // �^�X�NID
        string startTime = fields[2];                   // �J�n����
        string finishTime = fields[3];                  // �I������
        string memo = fields[4];                        // ����
        int progress = Convert.ToInt32(fields[5]);      // �i����

        // ���M�p��XML�f�[�^���쐬����
        XElement xml = new XElement("actualTime",
            new XElement("projectId", projectId),
            new XElement("taskId", taskId),
            new XElement("startTime", startTime),
            new XElement("finishTime", finishTime),
            new XElement("memo", memo)
        );

        // ���N�G�X�g���쐬����
        var client = new RestClient(url);
        var request = new RestRequest(resource, Method.POST);
        client.PreAuthenticate = true;
        request.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
        request.Credentials = new NetworkCredential(user, password);
        request.AddHeader("Content-Type", "application/xml");

        request.AddUrlSegment("id", accountId.ToString());
        request.AddXmlBody(xml);

        // ���X�|���X���擾����
        var result = client.Execute(request);
        idList.Add(result.Content); // �ǉ�����ID�����X�g�ɒǉ����Ă���


        // �^�X�N�̐i������o�^����

        // �X�V���e���`����i���蓖�ă^�X�N�̐i�������X�V�j
        xml = new XElement("assignedTask",
            new XElement("assignmentProgress", progress)
        );
        
        // ���N�G�X�g���쐬����
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

        // ���X�|���X���擾����
        result = client.Execute(request);
        string ret = result.Content;
    }
    return idList;
}

#endregion ���эH���ǉ��i�i�������܂ށj