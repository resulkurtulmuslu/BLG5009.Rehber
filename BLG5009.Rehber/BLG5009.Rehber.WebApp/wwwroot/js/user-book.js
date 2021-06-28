var userTable;

$(function () {
    getRandomStarUser();

    userTable = $('#userTable').DataTable({
        serverSide: true,
        processing: true,
        filter: true,
        ajax: {
            url: '/User/GetUsers',
            type: 'POST',
            dataType: 'json',
            async: true
        },
        rowId: "id",
        columns: [
            { data: "image", name: "image", className: "text-center", orderable: false },
            { data: "firstName", name: "firstName" },
            { data: "lastName", name: "lastName" },
            { data: "nickName", name: "nickName" },
            { data: "star", name: "star", className: "text-center" }
        ],
        columnDefs: [
            {
                targets: 0,
                render: function (data, display, row) {
                    return data == null ? "<img src='/image/user.png' height='35'>" : "<img src='/image/users/" + data + "' height='35'>";
                }
            },
            {
                targets: 4,
                render: function (data, display, row) {
                    return data
                        ? "<a href='#' onclick='setStar(" + row.id + "," + row.star + ")'><img src='/image/star.png' height='25'></img></a>"
                        : "<a href='#' onclick='setStar(" + row.id + "," + row.star + ")'><img src='/image/star-empty.png' height='25'></img></a>"
                }
            }
        ],
        language: {
            emptyTable: "Tabloda herhangi bir veri mevcut değil",
            info: "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            infoEmpty: "Kayıt yok",
            infoFiltered: "(_MAX_ kayıt içerisinden bulunan)",
            infoThousands: ".",
            lengthMenu: "Sayfada _MENU_ kayıt göster",
            loadingRecords: "Yükleniyor...",
            processing: "İşleniyor...",
            search: "Ara:",
            zeroRecords: "Eşleşen kayıt bulunamadı",
            paginate: {
                first: "İlk",
                last: "Son",
                next: "Sonraki",
                previous: "Önceki"
            },
            searchPlaceholder: "Ara"
        },
        initComplete: function () {
            userTableContextmenu();
        }
    });

    $(".dataTables_filter input").unbind().bind("input", function () {
        if (this.value.length >= 3) {
            userTable.search(this.value).draw();
        }
        else if (this.value == "") {
            userTable.search("").draw();
        }
        return;
    });
});

function userTableContextmenu() {
    $('#userTable').contextMenu({
        selector: 'tbody tr',
        trigger: 'right',
        events: {
            show: function (options) {
                $(this).addClass('selected');
            },
            hide: function (options) {
                $('#userTable tbody tr.selected').removeClass('selected');
            }
        },
        callback: function (key, options) {

            var data = $(this)[0];
            var id = data.id;

            switch (key) {
                case 'add':
                    getForm(null);
                    break;
                case 'update':
                    getForm(id);
                    break;
                case 'delete':
                    alertify.confirm('Rehber', 'Seçili öğeyi silmek istedüğünüze eminmisiniz ?',
                        function () {
                            $.ajax({
                                type: 'POST',
                                url: "/User/Delete",
                                data: { id },
                                dataType: "json",
                                async: true,
                                success: function (result) {
                                    userTable.ajax.reload(null, false);
                                    alertify.notify('İşlem Başarılı', 'success', 5);
                                },
                                error: function (ex) {
                                    alertify.notify(ex.responseText, 'error', 5);
                                }
                            });
                        },
                        function () {

                        }).set('labels', { ok: 'Evet Sil!', cancel: 'İptal' });
                    break;
            }
        },
        items: {
            "add": { name: "Yeni Kayıt", icon: "add" },
            "update": { name: "Düzenle", icon: "edit" },
            "delete": { name: "Kaldır", icon: "delete" },
        }
    })
}

function setStar(id, status) {
    $.ajax({
        type: 'POST',
        url: "/User/SetStar",
        data: { id, status },
        dataType: "json",
        async: true,
        success: function (result) {
            userTable.ajax.reload(null, false);
            getRandomStarUser();
            alertify.notify('İşlem Başarılı', 'success', 5);
        },
        error: function (ex) {
            alertify.notify(ex.responseText, 'error', 5);
        }
    });
}

function getForm(id) {
    $.ajax({
        type: 'GET',
        url: "/User/Form",
        data: { id },
        dataType: "html",
        async: true,
        success: function (result) {
            $('#systemModal').html(result);
            $('#systemModal').modal('show');
        },
        error: function (ex) {
            alertify.notify(ex.responseText, 'error', 5);
        }
    });
}

function getRandomStarUser() {
    $.ajax({
        type: 'GET',
        url: "/User/GetStarRandomUsers",
        dataType: "json",
        async: true,
        success: function (result) {
            $('#starRow').empty();

            $.each(result, function (i, v) {

                let image = v.image == null ? '/image/user.png' : '/image/users/' + v.image;

                let html = '<div class="col-3">' +
                    '<div class="card text-center">' +
                    '<img class="card-img-top" src="' + image  + '" alt="' + v.nickName + '" height="200">' +
                        '<div class="card-body">' +
                        '<h5 class="card-title">' + v.firstName + ' ' + v.lastName + '</h5>' +
                        '<a href="#" class="btn btn-primary">Git</a>' +
                        '</div>' +
                        '</div>' +
                        '</div>';

                $('#starRow').append(html);
            });
        },
        error: function (ex) {
            alertify.notify(ex.responseText, 'error', 5);
        }
    });
}