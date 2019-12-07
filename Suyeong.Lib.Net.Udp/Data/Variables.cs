namespace Suyeong.Lib.Net.Udp
{
    public static class Consts
    {
        // UDP 상에서 최대 전송 가능한 크기는 65,507 이다
        public const int SIZE_HEADER = 4; // UDP는 Type을 보내지 않는다.
        public const int SIZE_MAX = 65507;
    }
}
