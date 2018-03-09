// Write your JavaScript code.
function GoBack() {
    window.history.back();
}

function ChangeID(id, newID) {
    document.getElementById(id).id = newID;
}

function CopyToClipboard(id) {
    if (document.selection) {
        var range = document.body.createTextRange();
        range.moveToElementText(document.getElementById(id));
        range.select().createTextRange();
        document.execCommand("copy");

    } else if (window.getSelection) {
        var range = document.createRange();
        range.selectNode(document.getElementById(id));
        window.getSelection().addRange(range);
        document.execCommand("copy");
    }
}