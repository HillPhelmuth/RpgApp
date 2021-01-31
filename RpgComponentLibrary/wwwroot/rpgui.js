
var RPGUI = window.RPGUI || {};
let checkboxCount = 0;
let _update_funcs = {};
let _create_funcs = {};
let _get_funcs = {}
let _set_funcs = {};
let _init_list = [];
let _isInit = false;
export function initGui() {

    dispatchEvent(new Event('load'));

    console.log("RPGUI _init_list\r\n" + Object.entries(_init_list) + "\r\n_create_funcs created:\r\n" + Object.entries(_create_funcs) + "\r\n_set_funcs:\r\n" + Object.entries(_set_funcs) + "\r\n_get_funcs\r\n" + Object.entries(_get_funcs) + "\r\n_update_funcs:\r\n" + Object.entries(_update_funcs));

}
export function getGuiJsObject() {
    return "RPGUI functions created:\r\n" + Object.entries(_create_funcs) + "\r\n:::\r\n" + Object.entries(_set_funcs) + "\r\n:::\r\n" + Object.entries(_get_funcs) + "\r\n:::\r\n" + Object.entries(_update_funcs);
}
export function createDynamicList(elemId) {
    init();
    callOnLoad(function () {
        // get all containers and iterate them
        const contents = document.getElementsByClassName("rpgui-content");
        for (let i = 0; i < contents.length; ++i) {
            // get current container and init it
            const content = contents[i];

            // set default cursor
            RPGUI.set_cursor(content, "default");
        }
    });
    addCheckbox();
    // element currently dragged
    addDragDrop();
    // class name we will convert to special radio
    addRadio();
    addDropDown();
    addList();
    addSlider();
}

RPGUI = (function () {

    // lib version
    RPGUI.version = 1.03;

    // author
    RPGUI.author = "Ronen Ness";

    // if true, will init rpgui as soon as page loads
    // if you set to false you need to call init(); yourself.
    RPGUI.init_on_load = true;
    window.addEventListener("load", function () {
        if (RPGUI.init_on_load) { init(); }
    });

    // set & update the value of an element.
    // note: this function expect the original html element.
    //RPGUI.set_value = function (element, value) {
    //    // if have set value callback for this type, use it
    //    const type = element.dataset['rpguitype'];
    //    if (_set_funcs[type]) {
    //        _set_funcs[type](element, value);
    //    }
    //    // if not, use the default (setting "value" member)
    //    else {
    //        element.value = value;
    //    }

    //    // trigger update
    //    update(element);
    //    console.log("setValue(");
    //}

    // get the value of an element.
    // note: this function expect the original html element.
    RPGUI.get_value = function (element) {
        // if have get value callback for this type, use it
        console.log("RPGUI.get_value");
        const type = element.dataset['rpguitype'];
        if (_get_funcs[type]) {
            return _get_funcs[type](element);
        }
        // if not, use the default (getting the "value" member)
        else {
            return element.value;
        }

    }
  
    // init all the rpgui containers and their children
    callOnLoad(function () {
        // get all containers and iterate them
        const contents = document.getElementsByClassName("rpgui-content");
        for (let i = 0; i < contents.length; ++i) {
            // get current container and init it
            const content = contents[i];

            // set default cursor
            RPGUI.set_cursor(content, "default");
        }
    });
    addCheckbox();
    // element currently dragged
    addDragDrop();
    // class name we will convert to special radio
    addRadio();
    addDropDown();
    addList();
    addSlider();
    /**
    * Some helpers and utils.
    */

    RPGUI.set_cursor = function (element, cursor) {
        addClass(element, "rpgui-cursor-" + cursor);
    };

    return RPGUI;
})();
function init() {
    if (_isInit) { throw "RPGUI was already init!"; }
    for (let i = 0; i < _init_list.length; ++i) {
        _init_list[i]();
    }
   _isInit = true;
}
function create(element, rpgui_type) {
    if (_create_funcs[rpgui_type]) {
        element.dataset['rpguitype'] = rpgui_type;
        _create_funcs[rpgui_type](element);

    }
    // not a valid type? exception.
    else {
        throw "Not a valid rpgui type! options: " + Object.keys(_create_funcs);
    }
    console.log("create("+rpgui_type+")");
}
export function setValue(element, value) {
    // if have set value callback for this type, use it
    const type = element.dataset['rpguitype'];
    if (_set_funcs[type]) {
        _set_funcs[type](element, value);
    }
    // if not, use the default (setting "value" member)
    else {
        element.value = value;
    }

    // trigger update
    update(element);
    console.log(`setValue(${element}, ${value})`);
}
function update(element) {
    const type = element.dataset['rpguitype'];
    if (_update_funcs[type]) {
        _update_funcs[type](element);
    }
    // if not, use the default (firing update event)
    else {
        fireEvent(element, "change");
    }
    console.log("update(" + Object.entries(element)+(")"));
}
function callOnLoad(callback) {
    // if was already init call immediately
    if (_isInit) { callback(); }

    // add to init list
    _init_list.push(callback);
    console.log("callOnLoad()" + Object.values(callback));
}

//Helper functions:
function removeClass(element, cls) {
    element.className = (' ' + element.className + ' ').replace(cls, "");
    element.className = element.className.substring(1, element.className.length - 1);
}
function getChildWithClass(elem, cls) {
    for (let i = 0; i < elem.childNodes.length; i++) {
        if (hasClass(elem.childNodes[i], cls)) {
            return elem.childNodes[i];
        }
    }
    
}
function hasClass(element, cls) {
    return (' ' + element.className + ' ').indexOf(' ' + cls + ' ') > -1;
}
function addClass(element, cls) {
    if (!hasClass(element, cls)) {
        element.className += " " + cls;
    }
    
}
function insertAfter(to_insert, after_element) {
    after_element.parentNode.insertBefore(to_insert, after_element.nextSibling);
}
function fireEvent(element, type) {
    // http://stackoverflow.com/questions/2856513/how-can-i-trigger-an-onchange-event-manually
    if ("createEvent" in document) {
        const evt = document.createEvent("HTMLEvents");
        evt.initEvent(type, false, true);
        element.dispatchEvent(evt);
    }
    else {
        element.fireEvent("on" + type);
    }
    
}

function createElement(elemTag) {
    return document.createElement(elemTag);
}
function copyCss(from, to) {
    to.style.cssText = from.style.cssText;
}
function copyEventListeners(from, to) {
   
    if (typeof getEventListeners == "function") {
        const events = getEventListeners(from);
        for (var p in events) {
            if (Object.prototype.hasOwnProperty.call(events, p)) {
                events[p].forEach(function (ev) {
                    // {listener: Function, useCapture: Boolean}
                    to.addEventListener(p, ev.listener, ev.useCapture);
                });
            }
        }
    }

    // now copy all attributes that start with "on"
    for (let attr in from.attributes) {
        if (Object.prototype.hasOwnProperty.call(from.attributes, attr)) {
            if (attr.indexOf("on") === 0) {
                to[attr] = from[attr];
            }
        }
    }
    //};
}

function addCheckbox() {
    var _checkbox_class = "rpgui-checkbox";

    // create a rpgui-checkbox from a given element.
    // note: element must be <input> of type "checkbox" for this to work properly.
    _create_funcs["checkbox"] = function (element) {
        addClass(element, _checkbox_class);
        create_checkbox(element);
    };

    // set function to set value of the checkbox
    _set_funcs["checkbox"] = function (elem, value) {
        elem.checked = value;
    };

    // set function to get value of the checkbox
    _get_funcs["checkbox"] = function (elem) {
        return elem.checked;
    };

    // init all checkbox elements on page load
    callOnLoad(function () {
        // get all the input elements we need to upgrade
        const elems = document.getElementsByClassName(_checkbox_class);

        // iterate the selects and upgrade them
        for (let i = 0; i < elems.length; ++i) {
            create(elems[i], "checkbox");
        }
    });

    // upgrade a single "input" element to the beautiful checkbox class
    function create_checkbox(elem) {
        // get next sibling, assuming its the checkbox label.
        // this object will be turned into the new checkbox.
        checkboxCount = checkboxCount + 1;
        const new_checkbox = document.getElementById("checkbox-label-" + checkboxCount);

        // validate
        if (!new_checkbox || new_checkbox.tagName !== "LABEL") {
            console.log("After a '" + _checkbox_class + "' there must be a label!");
            return;
            //throw "After a '" + _checkbox_class + "' there must be a label!";
        }

        // copy all event listeners and events
        copyEventListeners(elem, new_checkbox);

        // do the click event for the new checkbox
        (function (elem, new_checkbox) {
            new_checkbox.addEventListener("click", function () {
                if (!elem.disabled) {
                    setValue(elem, !elem.checked);
                }

            });
        })(elem, new_checkbox);
    }
}

function addRadio() {
    var _radio_class = "rpgui-radio";

    // create a rpgui-radio from a given element.
    // note: element must be <input> of type "radio" for this to work properly.
    _create_funcs["radio"] = function (element) {
        addClass(element, _radio_class);
        create_radio(element);
    };

    // set function to set value of the radio
    _set_funcs["radio"] = function (elem, value) {
        elem.checked = value;
    };

    // set function to get value of the radio button
    _get_funcs["radio"] = function (elem) {
        return elem.checked;
    };

    // init all radio elements on page load
    callOnLoad(function () {
        // get all the input elements we need to upgrade
        const elems = document.getElementsByClassName(_radio_class);

        // iterate the selects and upgrade them
        for (let i = 0; i < elems.length; ++i) {
            create(elems[i], "radio");
        }
    });

    // upgrade a single "input" element to the beautiful radio class
    function create_radio(elem) {
        // get next sibling, assuming its the radio label.
        // this object will be turned into the new radio.
        const new_radio = elem.nextSibling;

        // validate
        if (!new_radio || new_radio.tagName !== "LABEL") {
            throw "After a '" + _radio_class + "' there must be a label!";
        }

        // copy all event listeners and events
        copyEventListeners(elem, new_radio);

        // do the click event for the new radio
        (function (elem, new_radio) {
            new_radio.addEventListener("click", function () {
                if (!elem.disabled) {
                    setValue(elem, true);
                }
            });
        })(elem, new_radio);
    }
}

function addDropDown() {
    var _dropdown_class = "rpgui-dropdown";

    // create a rpgui-dropdown from a given element.
    // note: element must be <select> with <option> tags that will turn into the items
    _create_funcs["dropdown"] = function (element) {
        addClass(element, _dropdown_class);
        create_dropdown(element);
    };

    // init all dropdown elements on page load
    callOnLoad(function () {
        // get all the select elements we need to upgrade
        const elems = document.getElementsByClassName(_dropdown_class);

        // iterate the selects and upgrade them
        for (let i = 0; i < elems.length; ++i) {
            create(elems[i], "dropdown");
        }
    });

    // upgrade a single "select" element to the beautiful dropdown
    function create_dropdown(elem) {
        // prefix to add arrow down next to selection header
        var arrow_down_prefix = "<label>&#9660;</label> ";

        // create the paragraph that will display the select_header option
        const select_header = createElement("p");
        if (elem.id) { select_header.id = elem.id + "-rpgui-dropdown-head"; };
        addClass(select_header, "rpgui-dropdown-imp rpgui-dropdown-imp-header");
        insertAfter(select_header, elem);

        // create the list to hold all the options
        const list = createElement("ul");
        if (elem.id) { list.id = elem.id + "-rpgui-dropdown"; };
        addClass(list, "rpgui-dropdown-imp");
        insertAfter(list, select_header);

        // set list top to be right under the select header
        const header_rect = select_header.getBoundingClientRect();
        list.style.position = "absolute";

        // set list width (-14 is to compensate borders)
        list.style.width = (header_rect.right - header_rect.left - 14) + "px";
        list.style.display = "none";

        // now hide the original select
        elem.style.display = "none";

        // iterate over all the options in this select
        for (let i = 0; i < elem.children.length; ++i) {
            // if this child is not option, skip
            const option = elem.children[i];
            if (option.tagName != "OPTION")
                continue;

            // add the new option as list item
            const item = createElement("li");
            addClass(item, "item");
            item.innerHTML = option.innerHTML;
            list.appendChild(item);

            // copy all event listeners from original option to the new item
            copyEventListeners(option, item);

            // set option callback (note: wrapped inside namespace to preserve vars)
            (function (elem, option, item, select_header, list) {
                // when clicking the customized option
                item.addEventListener('click', function () {
                    // set the header html and hide the list
                    select_header.innerHTML = arrow_down_prefix + option.innerHTML;
                    list.style.display = "none";

                    // select the option in the original selection
                    option.selected = true;
                    fireEvent(elem, "change");
                });

            })(elem, option, item, select_header, list);
        }

        // now set list and header callbacks
        // create a namespace to preserve variables
        (function (elem, list, select_header) {
            // when clicking the selected header show / hide the options list
            select_header.onclick = function () {
                if (!elem.disabled) {
                    const prev = list.style.display;
                    list.style.display = prev == "none" ? "block" : "none";
                }
            };

            // when mouse leave the options list, hide it
            list.onmouseleave = function () {
                list.style.display = "none";
            };

        })(elem, list, select_header);

        // lastly, listen to when the original select changes and update the customized list
        (function (elem, select_header, list) {
            // the function to update dropdown
            const _on_change = function () {
                // set the header html and hide the list
                if (elem.selectedIndex != -1) {
                    select_header.innerHTML = arrow_down_prefix + elem.options[elem.selectedIndex].text;
                }
                else {
                    select_header.innerHTML = arrow_down_prefix;
                }
                list.style.display = "none";
            };

            // register the update function and call it to set initial state
            elem.addEventListener('change', _on_change);
            _on_change();

        })(elem, select_header, list);
    }
}

function addList() {
    var _list_class = "rpgui-list";

    // create a rpgui-list from a given element.
    // note: element must be <select> with <option> tags that will turn into the items
    _create_funcs["list"] = function (element) {
        addClass(element, _list_class);
        create_list(element);
    };

    // init all list elements on page load
    callOnLoad(function () {
        // get all the select elements we need to upgrade
        const elems = document.getElementsByClassName(_list_class);

        // iterate the selects and upgrade them
        for (let i = 0; i < elems.length; ++i) {
            create(elems[i], "list");
        }
    });

    // upgrade a single "select" element to the beautiful list
    function create_list(elem) {
        // default list size is 3
        if (!elem.size)
            elem.size = 3;

        // create the list to hold all the options
        const list = createElement("ul");
        if (elem.id) { list.id = elem.id + "-rpgui-list"; };
        addClass(list, "rpgui-list-imp");
        elem.parentNode.insertBefore(list, elem.nextSibling);

        // now hide the original select
        elem.style.display = "none";

        // iterate over all the options in this select
        const all_items = [];
        for (let i = 0; i < elem.children.length; ++i) {
            // if this child is not option, skip
            const option = elem.children[i];
            if (option.tagName != "OPTION")
                continue;

            // add the new option as list item
            const item = createElement("li");
            item.innerHTML = option.innerHTML;
            list.appendChild(item);

            // set dataset value
            item.dataset['rpguivalue'] = option.value;

            // add to list of all items
            all_items.push(item);

            // copy all event listeners from original option to the new item
            copyEventListeners(option, item);

            // set option callback (note: wrapped inside namespace to preserve vars)
            (function (elem, option, item, list, all_items) {
                // when clicking the customized option
                item.addEventListener('click', function () {
                    // select the option in the original selection
                    if (!elem.disabled) {
                        option.selected = true;
                        fireEvent(elem, "change");
                    }
                });

            })(elem, option, item, list, all_items);
        }

        // if got any items set list height based on the size param
        if (all_items.length && elem.size) {

            // get the actual height of a single item in list
            const height = all_items[0].offsetHeight;

            // set list height based on size
            list.style.height = (height * elem.size) + "px";

        }

        // lastly, listen to when the original select changes and update the customized list
        (function (elem, all_items) {
            // handle value change
            elem.addEventListener('change', function () {
                _on_change(this);
            });
            function _on_change(elem) {
                for (let i = 0; i < all_items.length; ++i) {
                    const item = all_items[i];
                    if (item.dataset['rpguivalue'] == elem.value) {
                        addClass(item, "rpgui-selected");
                    }
                    else {
                        removeClass(item, "rpgui-selected");
                    }
                }
            }

            // call the on-change on init to set initial state
            _on_change(elem);

        })(elem, all_items);
    }
}

function addSlider() {
    var _slider_class = "rpgui-slider";

    // create a rpgui-slider from a given element.
    // note: element must be <input> of type "range" for this to work properly.
    _create_funcs["slider"] = function (element) {
        addClass(element, _slider_class);
        create_slider(element);
    };

    // init all slider elements on page load
    callOnLoad(function () {
        // get all the select elements we need to upgrade
        const elems = document.getElementsByClassName(_slider_class);

        // iterate the selects and upgrade them
        for (let i = 0; i < elems.length; ++i) {
            create(elems[i], "slider");
        }
    });

    // upgrade a single "input" element to the beautiful slider class
    function create_slider(elem) {
        // check if should do it golden slider
        const golden = hasClass(elem, "golden") ? " golden" : "";

        // create the containing div for the new slider
        const slider_container = createElement("div");
        if (elem.id) { slider_container.id = elem.id + "-rpgui-slider"; };
        copyCss(elem, slider_container);
        addClass(slider_container, "rpgui-slider-container" + golden);

        // insert the slider container
        insertAfter(slider_container, elem);

        // set container width based on element original width
        slider_container.style.width = elem.offsetWidth + "px";

        // create slider parts (edges, track, thumb)
        // track
        const track = createElement("div");
        addClass(track, "rpgui-slider-track" + golden);
        slider_container.appendChild(track);

        // left edge
        const left_edge = createElement("div");
        addClass(left_edge, "rpgui-slider-left-edge" + golden);
        slider_container.appendChild(left_edge);

        // right edge
        const right_edge = createElement("div");
        addClass(right_edge, "rpgui-slider-right-edge" + golden);
        slider_container.appendChild(right_edge);

        // thumb (slider value show)
        const thumb = createElement("div");
        addClass(thumb, "rpgui-slider-thumb" + golden);
        slider_container.appendChild(thumb);

        // hide original slider
        elem.style.display = "none";

        // copy events from original slider to container.
        // this will handle things like click, mouse move, mouse up, etc.
        // it will not handle things like "onchange".
        copyEventListeners(elem, slider_container);

        // now set events (wrap them in anonymous function to preserve local vars)
        const state = { mouse_down: false };
        (function (elem, slider_container, thumb, track, state, right_edge, left_edge) {
            // get the range of the original slider (min and max)
            var min = parseFloat(elem.min);
            var max = parseFloat(elem.max);

            // calculate edges width and track actual width
            var edges_width = right_edge.offsetWidth + left_edge.offsetWidth;
            var track_width = track.offsetWidth - edges_width;

            // set state if moving slider or not
            slider_container.addEventListener('mouseup', function (e) {
                state.mouse_down = false;
            });
            window.addEventListener('mouseup', function (e) {
                state.mouse_down = false;
            });
            track.addEventListener('mousedown', function (e) {
                state.mouse_down = true;
                slide(e.offsetX || e.layerX);
            });
            slider_container.addEventListener('mousedown', function (e) {
                state.mouse_down = true;
            });

            // handle clicking on edges (set to min / max)
            left_edge.addEventListener('mousedown', function (e) {
                set_value(min);
            });
            right_edge.addEventListener('mousedown', function (e) {
                set_value(max);
            });
            left_edge.addEventListener('mousemove', function (e) {
                if (state.mouse_down)
                    set_value(min);
            });
            right_edge.addEventListener('mousemove', function (e) {
                if (state.mouse_down)
                    set_value(max);
            });

            // handle sliding
            function slide(pos) {
                // calc new slider value
                const new_val = min + Math.round((pos / track_width) * (max - min)) - 1;

                // set thumb position
                set_value(new_val);
            }

            // setting value
            function set_value(new_val) {
                if (!elem.disabled &&
                    elem.value != new_val) {
                    setValue(elem, new_val);
                }
            }

            // moving the slider
            track.addEventListener('mousemove', function (e) {
                if (state.mouse_down && !elem.disabled) {
                    slide(e.offsetX || e.layerX);
                }
            });


            // when original slider value change update thumb position
            elem.addEventListener("change", function (e) {
                _onchange();
            });
            function _onchange() {
                // get the range of the original slider (min and max)
                const step = track_width / (max - min);
                const relative_val = Math.round(parseFloat(elem.value) - min);
                thumb.style.left = (Math.floor(edges_width * 0.25) + (relative_val * step)) + "px";
            }

            // call "_onchange()" to init the thumb starting position
            _onchange();

        })(elem, slider_container, thumb, track, state, right_edge, left_edge);

    }
}



export function addDragDrop() {
    var _curr_dragged = null;
    var _curr_dragged_point = null;
    var _dragged_z = 1000;

    // class name we consider as draggable
    var _draggable_class = "rpgui-draggable";

    // set element as draggable
    // note: this also add the "rpgui-draggable" css class to the element.
    _create_funcs["draggable"] = function (element) {
        // prevent forms of default dragging on this element
        element.draggable = false;
        element.ondragstart = function () { return false; };

        // add the mouse down event listener
        addClass(element, _draggable_class);
        element.addEventListener('mousedown', mouseDown);
        console.log("_create_funcs");
    };

    // init all draggable elements (objects with "rpgui-draggable" class)
    callOnLoad(function () {
        // init all draggable elements
        const elems = document.getElementsByClassName(_draggable_class);
        for (let i = 0; i < elems.length; ++i) {
            create(elems[i], "draggable");
        }

        // add mouseup event on window to stop dragging
        window.addEventListener('mouseup', mouseUp);
    });

    // stop drag
    function mouseUp(e) {
        _curr_dragged = null;
        window.removeEventListener('mousemove', divMove);
    }

    // start drag
    function mouseDown(e) {

        // set dragged object and make sure its really draggable
        const target = e.target || e.srcElement;
        if (!hasClass(target, _draggable_class)) { return; }
        _curr_dragged = target;
        // set holding point
        const rect = _curr_dragged.getBoundingClientRect();
        _curr_dragged_point = { x: rect.left - e.clientX, y: rect.top - e.clientY };
        // add z-index to top this element
        target.style.zIndex = _dragged_z++;
        // begin dragging
        window.addEventListener('mousemove', divMove, true);

    }

    // dragging
    function divMove(e) {
        if (_curr_dragged) {
            _curr_dragged.style.position = 'absolute';
            _curr_dragged.style.left = (e.clientX + _curr_dragged_point.x) + 'px';
            _curr_dragged.style.top = (e.clientY + _curr_dragged_point.y) + 'px';
        }
    }

    console.log("addDragDrop");
}
export function getWidth(element) {
    return element.offsetWidth;
}