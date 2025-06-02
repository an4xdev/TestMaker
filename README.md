# TestMaker

## About

The application will allow you to create tests, which will then be generated into an HTML page allowing for easy distribution.

### Requirements

- .NET Maui

### Run

Open in IDE I don't know another way to modify, compile and run this project.

## TODO

- [x] Full functionality in .NET Maui project
- [x] Move shared blazor components

## Plans

- [x] More interactive creation of questions
- [ ] Photos in
  - [ ] questions
  - [x] answers

## Parsing

Application in 2.0 release can parse md files like:

```md
# Project name (optional, if none project name is file name)

## Test question

- **Correct A answer**
- B answer
- C answer
- ![photo_description](path_to_photo)
- E answer

## Multi test question

- **Correct A**
- Wrong A
- **Correct C**
- Wrong D
- **![photo_description](path_to_photo)**
- Wrong F

## Open question

Lorem ipsum dolor sit amet, consectetur adipiscing elit.
Vestibulum mattis consectetur libero in facilisis.
Vivamus non mauris sit amet odio interdum sodales non in nisi.
Integer consequat purus risus, at pharetra dui condimentum eget.
Quisque nec tellus quis urna semper tempus.
Etiam facilisis maximus urna.
In sapien leo, mattis quis nulla in, scelerisque finibus sem.
Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.
Maecenas commodo libero odio, imperdiet feugiat sapien fringilla id.
Vestibulum eget nibh maximus, hendrerit neque eu, varius tortor.
Maecenas augue metus, facilisis ac massa ac, eleifend pellentesque dui.
Nulla facilisi.
Praesent euismod faucibus sagittis.
Pellentesque consectetur neque sed risus fermentum, quis blandit enim ultrices. 
```

After loading Markdown file with images you should see modal with question name and fields to upload photo.
