function selfRemovingTransitionEndPromise (element, callback) {
    return new Promise(function (resolve, reject) {
        function onTransitionEnd (evt) {
            element.removeEventListener("transitionend", onTransitionEnd);
            resolve(element);
        }

        // function onTransitionStart (evt) {
        //     element.removeEventListener(element, onTransitionStart);
        //     element.addEventListener("transitionend", onTransitionEnd);
        // }

        // element.addEventListener("transitionstart", onTransitionStart);
        element.addEventListener("transitionend", onTransitionEnd);
        if (typeof(callback) == "function")
            callback();
    });
}