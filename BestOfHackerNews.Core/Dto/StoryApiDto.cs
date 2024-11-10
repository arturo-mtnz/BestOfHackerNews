namespace BestOfHackerNews.Core.Dto;

internal class StoryApiDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string URL { get; set; } = string.Empty;
    public string By { get; set; } = string.Empty;
    public int Time { get; set; }
    public int Score { get; set; }
    public int[] Kids { get; set; } = [];
}
