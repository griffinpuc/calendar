# Calendar Web App
#### My submission for the AIM-HI coding challenge
<br />

## About this

This is a basic web-based weekly calendar. Users are assigned a unique GUID, which is stored in a cookie in their browser, this allows for persistent user data and assures that each user has a unique and private calendar.

### Features
- [x] Display weekly view
- [x] Navigate between weeks
- [x] Add, edit, and delete events
- [x] Persistent user events
- [x] Backend is not in cloud infastructure, but hosted myself on my home Windows Server. (Running Microsoft IIS webserver and MySQL server)

### Usage
I have self-hosted this webapp in a subdomain on my home webserver:<br />
Visit [the live webapp here](https://calendar.terramisha.com/)

You can click on blank hours to schedule events, hover over events to see descriptions, and click events to edit them.

### Building from Source
Steps here for building from source:<br />
<br />
Download Source [Source available here](https://github.com/griffinpuc/calendar/blob/master/SQL%20Models/create.sql)<br />
Configure MySQL Database [File available here](https://github.com/griffinpuc/calendar/blob/master/SQL%20Models/create.sql)<br />
Publish to IIS webserver<br />
