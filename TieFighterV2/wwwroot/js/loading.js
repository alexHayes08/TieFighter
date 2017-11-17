// Add loading styling
var headEl = document.getElementsByTagName("head")[0];
var styleEl = document.createElement("style");
styleEl.innerHTML = 
`.spinner {
    display: none;
}
[loading] {
    position: inherit;
}

    [loading]::after {
        background-color: rgba(0,0,0,0.25);
        content: "";
        position: absolute;
        top: 0%;
        left: 0%;
        height: 100%;
        width: 100%;
    }

[loading] > [spinner] {
    border: 4px solid white;
    border-top-color: green;
    border-bottom-color: green;
    border-radius: 50%;
    box-sizing: border-box;
    display: block;
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%,-50%);
    animation: 1s infinite linear spinning;
}
@keyframes fadeOutAnim {
    0% {
        opacity: 1;
        height: 100%;
        width: 100%;
    }
    75% {
        opacity: 0;
        height: 100%;
        width: 100%;
        line-height: 1.5;
    }
    /*99% {
        opacity: 0;
        height: 0%;
        width: 0%;
    }*/
    100% {
        /*display: none;*/
        opacity: 0;
        line-height: 0px;
        height: 0%;
        width: 0%;
    }
}`;

headEl.appendChild(styleEl);

function addLoading(element, spinner) {
    var spinnerEl = spinner;
    if (spinnerEl == null) {
        spinnerEl = document.createElement("div");
        spinnerEl.setAttribute("spinner", "");
        var elementBB = element.getBoundingClientRect();
        if (elementBB.width > elementBB.height) {
            spinnerEl.style.height = "33%"
        } else {
            spinnerEl.style.width = "33%"
        }
    }

    if (!element.hasAttribute("loading")) {
        element.setAttribute("loading", "");
    }
    element.appendChild(spinnerEl);
}

function removeLoading(element, spinner) {
    element.removeAttribute("loading");
    if (spinner != null) {
        element.removeChild(spinner);
    } else {
        var spinners = Array
            .from(element.children)
            .filter(el => el.hasAttribute("spinner"));
        element.removeChild(spinners.pop());
    }
}

(function () {
    var loadables = document.querySelectorAll("[loading]");
    for (var loadable of loadables) {
        addLoading(loadable);
    }
})();