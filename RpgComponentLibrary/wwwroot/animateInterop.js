// This is a JavaScript module that is just handles the keyboard events ('keydown' and 'keyup') Adds an event listener over the whole browser window so if canvas element loses focus, the animation keys will still work.
let dotNetInstance = null;
export function showPrompt(message) {
  return prompt(message, 'Type anything here');
}
export function setEventListeners(instance) {
    dotNetInstance = instance;
    window.addEventListener('keydown', keyDownListener, false);
    window.addEventListener('keyup', keyUpListener, false);
}
function keyDownListener(event) {
    console.log("key press: " + event.key);
    dotNetInstance.invokeMethodAsync('HandleKeyDown', event.key);
}
function keyUpListener(event) {
    dotNetInstance.invokeMethodAsync('HandleKeyUp', event.key);
}
export function dispose() {
    dotNetInstance.dispose();
    window.removeEventListener('keyup', keyUpListener, false);
    window.removeEventListener('keydown', keyDownListener, false);
}
export function ping() {
    console.log("import animateInterop.js");
}
