# SharpKoreanBots
한국 디스코드 리스트의 C# SDK

이것은 한국 디스코드 리스트의 **비공식** C# SDK입니다.

한국 디스코드 리스트의 API 버전은 2입니다.

---
## 설치
```
dotnet add package SharpKoreanBot
```
---
## 사용법
### 1. 봇 정보 가져오기
```cs
using SharpKoreanBots.Bot;

BotInfo info = BotInfo.Get(봇 ID);
Console.WriteLine(info); //출력: 이름#태그
```
### 2. 봇 정보 업데이트하기
```cs
using SharpKoreanBots.Bot;

BotInfo info = BotInfo.Get(봇 ID);
info.Token = "봇 토큰";
info.ServerCount = 서버수;
info.ShardCount = 사드수;
info.Update();
```
### 3. 유저 정보 가져오기
```cs
using SharpKoreanBots.User;

UserInfo info = UserInfo.Get(유저 ID);
Console.WriteLine(info) //출력: 이름#태그
```
### 4. 유저가 투표를 했는지 확인하기
```cs
using SharpKoreanBots.Bot;
using SharpKoreanBots.User;

BotInfo bInfo = new BotInfo(봇ID, "TOKEN"); //BotInfo.Get하고 Token 설정해도 됨
UserInfo uInfo = UserInfo.Get(유저ID);
Console.WriteLine(binfo.isVoted(uInfo)); //uInfo 대신 유저ID 넣을 수도 있음
//출력: True 혹은 False
```
### 5. 위젯 가져오기
```cs
using SharpKoreanBots.Widget;

Widget.DownloadWidget("PATH", 봇ID, WidgetType.Servers); //서버 위젯 가져오기(기본 설정)
```

예시는 [여기](https://github.com/csnewcs/SharpKoreanBots/tree/main/SharpKoreanBots/example)를 참고해주세요.
