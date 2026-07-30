function checkSessionStorage() {
    
    return sessionStorage.getItem('SessionUsuario') !== null;
};

function downloadFileFromStream(fileName, contentStreamReference) {
    contentStreamReference.arrayBuffer().then(function (buffer) {
        var blob = new Blob([buffer]);
        var url = URL.createObjectURL(blob);
        var anchorElement = document.createElement('a');
        anchorElement.href = url;
        anchorElement.download = fileName;
        anchorElement.click();
        anchorElement.remove();
        URL.revokeObjectURL(url);
    });
}