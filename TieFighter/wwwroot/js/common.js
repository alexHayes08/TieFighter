class EventListenerWrapper {
    constructor(element, on, func) {
        this._element = element;
        this._on = on;
        this._func = func;
        this._isDestroyed = false;
        element.addEventListener(on, func);
    }
    get isDestroyed() {
        return this._isDestroyed;
    }
    destroy() {
        if (!this.isDestroyed) {
            this._element.removeEventListener(on, func);
            this._isDestroyed = true;
        }
    }
}

function isLoading(element, boolean) {
    if (boolean) {
        element.setAttribute("loading", "");
    } else {
        element.addEventListener("animationend", function () {
            element.removeAttribute("finishedLoading");
            element.removeAttribute("loading");
        }, { once: true });
        element.setAttribute("finishedLoading", "");
    }
}

function pageLoading(boolean) {
    const pageLoadingTemplate = $.parseHTML(`
        <template>
            <div class="horizontalLoadingIndicator">
                <div class="colorA"></div>
                <div class="colorB"></div>
                <div class="colorC"></div>
                <div class="colorD"></div>
            </div>
        </template>`);
    var $header = $("header");
    if (boolean) {
        var content = pageLoadingTemplate[1].content.cloneNode(true);
        $header.append(content);
    } else {
        $header.find(".horizontalLoadingIndicator").remove()
    }
}

/**
 * Returns a new Promise when either the websocket connection succeeds or fails.
 * @Returns Promise that resolves/rejects when the websocket responds
 */
function connectToWebsocket() {
    return new Promise(function (resolve, reject) {
        var scheme = document.location.protocol == "https:" ? "wss" : "ws";
        var port = document.location.port ? (":" + document.location.port) : "";
        var connectionUrl = scheme + "://" + document.location.hostname + port + "/ws";
        var socket = new WebSocket(connectionUrl.value);

        function onClose() {
            socket.removeEventListener("close", onClose);
            socket.removeEventListener("open", onOpen);
            reject("Connection was closed or rejected");
        }

        function onOpen() {
            socket.removeEventListener("close", onClose);
            socket.removeEventListener("open", onOpen);
            resolve(socket);
        }

        socket.addEventListener("close", onClose);
        socket.addEventListener("open", onOpen);
    });
}

$(function () {
    $("#leftNavBar i[data-toggle]").each(function () {
        var $target = $(this).attr("href");
        $target = $($target);
        var onShowFunc = function () {
            this.removeClass("fa-arrow-circle-down");
            this.addClass("fa-arrow-circle-up");
        }.bind($(this));
        var onHideFunc = function () {
            this.removeClass("fa-arrow-circle-up");
            this.addClass("fa-arrow-circle-down");
        }.bind($(this));
        $target.on("show.bs.collapse", onShowFunc);
        $target.on("hide.bs.collapse", onHideFunc);
    });

    $('[data-toggle="tooltip"]').tooltip({
        delay: 500,
        trigger: "hover"
    });

    $(".scrollTo").each(function () {
        var $this = $(this);
        $this.on("click", function () {
            var bbox = $($this.attr("data-scrollTo"))[0].getBoundingClientRect();
            window.scroll({
                top: bbox.top - 62 + window.scrollY,
                left: 0,
                behavior: "smooth"
            });
        });
    });
});