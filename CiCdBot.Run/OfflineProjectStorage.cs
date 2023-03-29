using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiCd.Domain;
using System.IO;
using System.Text.Json;

namespace CiCdBot.Run;

public class OfflineProjectStorage : IProjectStorage
{

    private IList<ProjectChat> _chats = null;
    private object lockObject = new object();

    public OfflineProjectStorage()
    {
        if (_chats == null)
            lock (lockObject)
                if (_chats == null)
                {
                    if (File.Exists("Data.json") == false)
                        _chats = new List<ProjectChat>();
                    else
                    {
                        using var fs = new FileStream("Data.json", FileMode.Open, FileAccess.Read);

                        var data = JsonSerializer.Deserialize<List<ProjectChat>>(fs, JsonSerializerOptions.Default);

                        fs.Close();
                        _chats = data;
                    }
                }

    }

    public ProjectChat GetProjectChat(long chatId)
    {
        return _chats.FirstOrDefault(x => x.Id == chatId);
    }


    public Task SaveAsync()
    {
        return Task.Run(() =>
        {
            lock (lockObject)
            {
                using var fs = new FileStream("Data.json", FileMode.Create, FileAccess.Write);
                JsonSerializer.Serialize(fs, _chats);
                fs.Close();
            }
        });
    }

    public void SetProjectChat(ProjectChat projectChat)
    {
        var chat = _chats.FirstOrDefault(x => x.Id == projectChat.Id);
        if (chat == null)
        {
            _chats.Add(projectChat);
            return;
        }
        if (projectChat.Equals(chat))
            return;

        _chats.Remove(chat);
        _chats.Add(chat);
    }
}
