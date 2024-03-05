# LocalChat
An application that allows you to chat with others in LAN.<br>
Also a Godot network system practice<br>
Made with Godot 4 Mono<br><br>
I made this because i want to chat with my classmates in information technology class across the classroom without public network enabled.<br>
My teachers are really fond of unplugging the network cable between switcher and the router.

## Environment Dependence
Godot 4.2.1 Stable Mono Official<br>
.NET 8.0.100 (well actually, .net8 is not necessary, if you want to use other version of .net, just change the `TargetFramework` in `LocalChat.csproj`)<br>
Inno Setup (If you want to compile ```.exe``` installer)

## Open-source License
MIT License

## To-do List
- [x] Enter to Send
- [x] Message Time
- [x] Localization
- [x] Sounds (From [Mixkit](https://mixkit.co))
- [x] Member List
- [x] Copy Message
- [x] Delete Message
- [x] Remove Member
- [x] Add Version Check
- [x] Add Sha256 Check
- [x] Add Installer
- [x] Taskbar alert when received new message
- [x] Scroll to bottom when received new message
- [x] Add scroll bar in Main Menu to avoid too much IP addresses
- [x] Text box go to top when open Input method in mobile
- [x] Add desktop notification (windows)
- [x] Add desktop notification (linux)
- [x] Allow user to ping someone
- [ ] Sending Image
- [ ] ~~Add a Chat Room lobby, add public and private option~~ Since this app works with UDP and p2p connection, i can't make it possible
- [ ] ~~Add notification (android)~~ Since Godot can't make app runs in background in Android without coding in Java, i gave up
- [ ] ~~Sending Files~~ Use [LocalSend](https://localsend.org) instead