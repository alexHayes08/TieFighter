(function () {
    const template = document.createElement('template');
    template.innerHTML = `
    <style>
        @keyframes loadingSpinner {
            from { transform: rotate(0deg); }
            to { transform: rotate(360deg); }
        }
        @keyframes pulsingMask {
            from { background: radial-gradient(circle at center, rgba(247, 255, 247, .75) 0, blue 100%) }
            to { background: radial-gradient(circle at center, blue 0, rgba(247, 255, 247, .75) 100%) }
        }
        :host {
            display: table-cell;
            position: relative;
        }
        :host > #mask {
            /*animation: pulsingMask alternate 4000ms linear infinite;*/
            background: radial-gradient(circle at center, rgba(247, 255, 247, .75) 0, rgba(237, 248, 237, 0.75) 100%);
            overflow: hidden;
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;

            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
        }
        :host > #mask > #loadingSpinner {
            animation: loadingSpinner 1000ms infinite linear;
            background-color: rgba(0,0,0,0);
            border: solid rgba(0,0,0,0) 10px;
            border-left-color: rgba(78, 205, 196, 1);
            border-bottom-color: rgba(78, 205, 196, 1);
            border-right-color: bisque;
            border-top-color: bisque;
            border-radius: 50%;
            box-sizing: border-box;
        }
    </style>`;

    class XLoadable extends HTMLElement {
        constructor () {
            super();

            // Create a shadow root
            this.attachShadow({mode: 'open'});
            this.shadowRoot.appendChild(template.content.cloneNode(true));

            var content = document.createElement("slot");
            this.shadowRoot.appendChild(content);

            var spinner = document.createElement("div");
            spinner.setAttribute("id", "loadingSpinner");

            var mask = document.createElement("div");
            mask.setAttribute("id", "mask");
            
            mask.appendChild(spinner);
            this.shadowRoot.appendChild(mask);
        }
        connectedCallback () {
            this.loading = this.hasAttribute("loading");
            // ResizeObserverService.observe({target: this, callback: this._onResize.bind(this)});
            ResizeObserverService.observe(this);
            this._onResize();
        }
        disconnectedCallback () {
            this.removeEventListener("resize", this._onResize)
        }
        _onResize () {
            var bb = this.getBoundingClientRect();
            var spinner = this.shadowRoot.getElementById("loadingSpinner");
            var side = 0;
            if (bb.width > bb.height) {
                side = bb.height * .75;
            } else {
                side = bb.width * .75;
            }
            if (side < 50) {
                side = 0;
            }

            spinner.style.height = `${side}px`;
            spinner.style.width = `${side}px`;
            // spinner.style.left = (bb.width - side) / 2;
            // spinner.style.top = (bb.height - side) / 2;
        }
        get loading () {
            return this.hasAttribute("loading");
        }
        set loading (value) {
            var val = Boolean(value);
            if (val) {
                this.shadowRoot.getElementById("mask").removeAttribute("style");
                // this.shadowRoot.getElementById("loadingSpinner").style.display = "block";
                if (!this.hasAttribute("loading")) {
                    this.setAttribute("loading", "");
                    this.dispatchEvent(new CustomEvent("loading", {detail: true}));
                }
            } else {
                this.shadowRoot.getElementById("mask").style.display = "none";
                // this.shadowRoot.getElementById("loadingSpinner").style.display = "none";
                if (this.hasAttribute("loading")) {
                    this.removeAttribute("loading");
                    this.dispatchEvent(new CustomEvent("loading", {detail: false}));
                }
            }
        }
        static get observedAttributes () {
            return ["loading"];
        }
        attributeChangedCallback(attr, oldValue, newValue) {
            if (attr == "loading") {
                // console.log(`${attr} was changed from ${oldValue} to ${newValue}.`);
                if (newValue == null) {
                    this.loading = false;
                } else {
                    this.loading = true;
                };
            }
        }
    }

    customElements.define("x-loadable", XLoadable);
})();