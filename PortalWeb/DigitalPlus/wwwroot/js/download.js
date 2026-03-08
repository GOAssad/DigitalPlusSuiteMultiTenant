window.saveAsFile = function (filename, byteBase64) {
    var link = this.document.createElement('a');
    link.download = filename;
    link.href = "data:appliction/octet-stream;base64," + byteBase64;
    this.document.body.appendChild(link);
    link.click();
    this.document.body.removeChild(link);
}