public interface ITween
{
    void Play(bool reverse = false);
    void Stop();
    void Reset(bool reverse = false);
}