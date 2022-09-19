
function invoke() {
    var macy = Macy({
        container: document.querySelector(".thoughts-container"),
        trueOrder: false,
        waitForImages: false,
        margin: 24,
        columns: 3,
        breakAt: {
            600: 2,
            430: 1
        }
    });
}

document.addEventListener('DOMContentLoaded', invoke, false);
