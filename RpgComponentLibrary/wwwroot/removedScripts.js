// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function showPrompt(message) {
  return prompt(message, 'Type anything here');
}
// Handled in html/css/c#
function addProgress() {
    var _progress_class = "rpgui-progress";

    // create a rpgui-progress from a given element.
    // note: element must be <input> of type "range" for this to work properly.
    _create_funcs["progress"] = function (element) {
        addClass(element, _progress_class);
        create_progress(element);
    };

    // set function to set value of the progress bar
    // value should be in range of 0 - 1.0
    _set_funcs["progress"] = function (elem, value) {
        // get trackbar and progress bar elements
        const track = getChildWithClass(elem, "rpgui-progress-track");
        const progress = getChildWithClass(track, "rpgui-progress-fill");

        // get the two edges
        const edge_left = getChildWithClass(elem, "rpgui-progress-left-edge");
        const edge_right = getChildWithClass(elem, "rpgui-progress-right-edge");

        // set progress width
        progress.style.left = "0px";
        progress.style.width = (value * 100) + "%";
    };

    // init all progress elements on page load
    callOnLoad(function () {
        // get all the select elements we need to upgrade
        const elems = document.getElementsByClassName(_progress_class);

        // iterate the selects and upgrade them
        for (let i = 0; i < elems.length; ++i) {
            create(elems[i], "progress");
        }
    });

    // upgrade a single "input" element to the beautiful progress class
    function create_progress(elem) {
        // create the containing div for the new progress
        const progress_container = elem;

        // insert the progress container
        insertAfter(progress_container, elem);

        // create progress parts (edges, track, thumb)
        // track
        const track = createElement("div");
        addClass(track, "rpgui-progress-track");
        progress_container.appendChild(track);

        // left edge
        const left_edge = createElement("div");
        addClass(left_edge, "rpgui-progress-left-edge");
        progress_container.appendChild(left_edge);

        // right edge
        const right_edge = createElement("div");
        addClass(right_edge, "rpgui-progress-right-edge");
        progress_container.appendChild(right_edge);

        // the progress itself
        const progress = createElement("div");
        addClass(progress, "rpgui-progress-fill");
        track.appendChild(progress);

        // set color
        if (hasClass(elem, "blue")) { progress.className += " blue"; }
        if (hasClass(elem, "red")) { progress.className += " red"; }
        if (hasClass(elem, "green")) { progress.className += " green"; }

        // set starting default value
        const starting_val = elem.dataset.value !== undefined ? parseFloat(elem.dataset.value) : 1;
        setValue(elem, starting_val);
    }
}