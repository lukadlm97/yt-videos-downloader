# Project description
VideoAndAudioDownloader is a suite of projects that enables users to download and save videos and playlists from YouTube to their local file system. It is built using .NET 6 and utilizes dependency injection (DI). The core business logic is contained in a library project, which can be accessed in two different ways: through a command-prompt console app or via a user-friendly desktop app built with WPF.

## VideoAndAudioDownloader.BusinessLogic
VideoAndAudioDownloader.BusinessLogic is a project that provides the business logic for fetching YouTube assets and saving them to the file system. It uses the YoutubeExplode library to retrieve details of individual videos and playlists, as well as to manipulate the assets for storage on the file system. This project is essential for the functionality of the VideoAndAudioDownloader app, as it handles all of the behind-the-scenes work required to fetch and save YouTube content. With its robust and reliable logic, VideoAndAudioDownloader.BusinessLogic ensures a seamless experience for the user.

## VideoAndAudioDownloader.ConsoleApp
VideoAndAudioDownloader.ConsoleApp is a self-hosted console application that utilizes the functionality provided by the VideoAndAudioDownloader.BusinessLogic project. It allows the user to execute the app from the command prompt, providing a convenient and flexible way to use the app's features. The console app is enhanced with the use of the ConsoleAppFramework library, which enables advanced parameterization of the app's execution. This allows for a more powerful and customizable experience when using the app from the command line. Overall, VideoAndAudioDownloader.ConsoleApp provides a robust and user-friendly interface for accessing the capabilities of the VideoAndAudioDownloader app.

## VideoAndAudioDownloader.Desktop
This is a desktop app built with WPF and .NET 7. It follows the MVVM design pattern and has three main windows. The first window allows you to import a YouTube playlist and perform a search to return all of the songs/videos in the playlist. You can also remove certain items from the playlist. The second window allows you to add more playlist and songs using a helper user control. Finally, the third window allows you to select one or more folders as destinations where all of the songs from the playlist will be saved to the file system.

## Desktop app demo
### Start screen:
<img width="591" alt="start-screen" src="https://user-images.githubusercontent.com/36825550/211218113-6e9fecb6-f45b-43be-8eb3-a10433f5dfce.png">

### Simple use case:
<img width="587" alt="simple-use-case" src="https://user-images.githubusercontent.com/36825550/211218133-09fbe23f-1e45-461a-b4c6-8534eff271f9.png">

### Add new song to playlist:
<img width="627" alt="add-new-song" src="https://user-images.githubusercontent.com/36825550/211218141-9e100881-f9ab-457e-90f4-aede34da2962.png">

### Select destinations:
<img width="473" alt="select-destinations" src="https://user-images.githubusercontent.com/36825550/211218152-3b956c60-ac2c-4722-a585-72125f75d7f3.png">

### Another pop-up for selecting exact folder path:
<img width="473" alt="select-single-destination" src="https://user-images.githubusercontent.com/36825550/211218178-57578aa4-9c0b-4c41-998e-0f98e97c2f0a.png">

### Example of downloaded videos:
<img width="394" alt="example-of-downloaded-videos" src="https://user-images.githubusercontent.com/36825550/211218216-4ee32a51-9b23-4e81-98da-ae2d64232613.png">

### Example of donwloaded audios:
<img width="390" alt="audio-example" src="https://user-images.githubusercontent.com/36825550/211218238-7b680a92-0007-4565-b4b1-a97615b53f04.png">




