using System.Threading.Tasks;
using CiCd.Domain;

namespace CiCdBot.Run;

public interface IProjectStorage
{
    public ProjectChat GetProjectChat(long chatId);
    public void SetProjectChat(ProjectChat projectChat);
    public Task SaveAsync();
}

