namespace ChurchPresenter.UI.Models.Folder
{
    public enum FolderType
    {
        Lyric,
        Scripture
    }

    public interface IFolder
    {
        FolderType GetFolderType();
        string GetTitle();
        Slide[] GetSlides();
    }
}
