function confirmar(title, text, icon) {
    return new Promise(resolve => {
        Swal.fire({
            title,
            text,
            icon,
            toast:true,
            showCancelButton: true,
            cancelButtonText:'Cancelar',
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, Borrarlo!'

        }).then((result) => {
            resolve(result.isConfirmed);
        })
    })
    
}

function ConfirmarSinBotones(title, texto) {
    return new Promise(resolve => {
        Swal.fire({
            position: 'top-end',
            icon: 'success',
            title: title,
            text: texto,
            showConfirmButton: false,
            timer: 1500
        }).then((result) => {
            resolve(result.isConfirmed);
        })
    })
}

function SiNo(title, text, textoBotonSi) {
    return new Promise(resolve => {
        Swal.fire({
            title,
            text,
            icon: 'question',
            showCancelButton: true,
            cancelButtonText: 'No',
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: textoBotonSi

        }).then((result) => {
            resolve(result.isConfirmed);
        })
    })
}


function SaveAsFile(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytesBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}