# 2022-1-CECD3-SaladBall-1
테스트베드 기반 클라우드 기반 실감형 메타버스 캠퍼스 테스트베드 구축

## Project
- 메타버스 캠퍼스 테스트베드 구축  
- 매치메이킹 및 게임 플레이 서버 구축 
- 동국대학교 메타버스 캠퍼스 맵 구현  
- 사용자의 오브젝트 및 모드 디자인 참여를 위한 시스템 구축  

## 개발 환경
> Unity 2021.3.1f1   
> Mirror Network 66.0.9   
> AWS  
> 3D-MAX  
> Google Site

## 실행 방법
1. Unity 설치 및 AWS EC2 인스턴스 생성
2. GitHub 레포 download 및 open
3. Offline Scene - NetworkManager - Network Address에 AWS 주소 입력
4. File - Build Settings 클릭 - Scenes In Build에 Assets - Karting - Scenes의 모든 Scene 올리기   
(IntroMenu-Offline-Lobby-Game0-Game1-Game2-WinScene-LoseScene 순서로)
5. Platform(Dedicated Server) - Target Platform(Linux) - `Build_Server` 폴더 생성 후 Build   
(IntroMenu, WinScene, LoseScene은 체크 해제)
6. Platform(Windows, Mac, Linux) - Target Platform(Windows) - `Build` 폴더 생성 후 Build
(모든 Scene 체크)
7. FTP 등을 이용해 AWS에 `Build_Server` 폴더 업로드 후 다음 명령어 입력(파일 명은 다를 수 있음)
   ```bash
   sudo chmod +x *
   ./Build_Server.x86_64*
   ```
8. `Build` 폴더의 Unity.exe 실행 

## Structure 
![structure](https://user-images.githubusercontent.com/90669873/206856569-289e9343-29bb-4ff6-85be-184f0f854954.png)   

## 주요 기능 및 실행 화면
- 서버 연결 후 LobbyScene  
![lobby](https://user-images.githubusercontent.com/90669873/206862169-323934e3-1fc3-4da0-98a8-99abd9bb3ebf.png)   

- 매치 생성 후 Waiting Room Scene  
![room](https://user-images.githubusercontent.com/90669873/206862475-d49dc29a-d178-415a-94b6-61acf6831d27.png)   

- 게임 Scene  
![game](https://user-images.githubusercontent.com/90669873/206862171-45c0ce71-4dab-4560-9fa3-19ca2a5dcf4e.png)   

- 동국대학교 캠퍼스 맵   
![map](https://user-images.githubusercontent.com/90669873/206862235-cd2fe635-24df-421d-a660-2164672d84a8.png)   

- 사용자의 디자인 참여를 위한 웹 페이지   
![web](https://user-images.githubusercontent.com/90669873/206862544-9802e537-aea6-4030-904b-5135d26ed027.png)   

## 팀원 및 문의
| 이름 | 역할 | 연락처 |
|---|---|---|
|권상혁|Web|2015110720@dgu.ac.kr|
|배승혁|Object(Map)|mvash.email@gmail.com|
|전유진|Object(Kart)|lenchanti20@gmail.com|
|황태윤|Server, Game|htymagpie@dgu.ac.kr|

## Reference
+ [https://mirror-networking.com/](https://mirror-networking.com/)
+ [https://unity.com/](https://unity.com/)

## License
[MIT](https://choosealicense.com/licenses/mit/)
