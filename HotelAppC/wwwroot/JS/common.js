

ShowToastr = (type, message) => {
    if (type === 'success') { toastr.success(message, 'HotelApp'); }
    if (type === 'error') { toastr.error(message, 'HotelApp'); }
    

}


SwalConfirm = () => {
    return new Promise(resolve => {
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            resolve(result.isConfirmed);
            if (result.isConfirmed) {
                Swal.fire(
                    'Deleted!',
                    'Your file has been deleted.',
                    'success'
                )
            }
        })
    })
}

printReport = (verb) => {
    var data = document.getElementById('invoice');
    html2canvas(data).then(canvas => {
        var imgWidth = 250;
        var imgHeight = canvas.height * imgWidth / canvas.width;
        const contentDataURL = canvas.toDataURL('image/png')
        let pdf = new jsPDF('l', 'mm', 'a4');
        pdf.addImage(contentDataURL, 'PNG', 22, 12, imgWidth, imgHeight)

        if (verb === 'print') {
            window.open(pdf.output('bloburl'), '_blank');
        }
        if (verb === 'download') {
            pdf.save('Invoice.pdf');
        }

    })
};



