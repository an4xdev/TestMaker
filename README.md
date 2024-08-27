# TestMaker

## About

The application will allow you to create tests, which will then be generated into an HTML page allowing for easy distribution.

## Quick start

For now implementation is focused in TestMaker.Hybrid project.

### Requirements

- .NET Maui

### Run

Run *.Hybrid project in IDE, which will also compile *.Data project.

## TODO

- [ ] Full functionality in .NET Maui project
- [ ] Move shared blazor components

## Plans

- [ ] More interactive creation of questions
- [ ] Photos in questions and answers

## Parsing

Application now can parse md files like:

```md
# Project name (optional, if none project name is file name)

<!-- Question with one and multiple answers must now have exactly 4 answers -->

## Test question

- **Correct A answer**
- B answer
- C answer
- D answer

## Multi test question

- **Correct A**
- Wrong A
- **Correct C**
- Wrong D

## Open question

(**asnwer must be in one line**)
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum mattis consectetur libero in facilisis. Vivamus non mauris sit amet odio interdum sodales non in nisi. Integer consequat purus risus, at pharetra dui condimentum eget. Quisque nec tellus quis urna semper tempus. Etiam facilisis maximus urna. In sapien leo, mattis quis nulla in, scelerisque finibus sem. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Maecenas commodo libero odio, imperdiet feugiat sapien fringilla id. Vestibulum eget nibh maximus, hendrerit neque eu, varius tortor. Maecenas augue metus, facilisis ac massa ac, eleifend pellentesque dui. Nulla facilisi. Praesent euismod faucibus sagittis. Pellentesque consectetur neque sed risus fermentum, quis blandit enim ultrices. 
```
