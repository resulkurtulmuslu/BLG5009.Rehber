var telephoneTable;

function getTelephoneTable(userId) {
    telephoneTable = $('#telephoneTable').DataTable({
        serverSide: true,
        processing: true,
        filter: true,
        ajax: {
            url: '/Telephone/GetTelephoneNumbersByUserId',
            type: 'POST',
            data: { userId },
            dataType: 'json',
            async: true
        },
        dom: 'Bfrtip',
        buttons: [
            {
                text: 'Yeni Kayıt',
                action: function (e, dt, node, config) {
                    getTelephoneForm(userId, null);
                }
            },
            {
                text: 'Geri',
                action: function (e, dt, node, config) {
                    window.history.back();
                }
            }
        ],
        rowId: "id",
        columns: [
            { data: "number", name: "number" },
            { data: "typeText", name: "typeText" },
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
            telephoneTableContextmenu(userId);
        }
    });

    $(".dataTables_filter input").unbind().bind("input", function () {
        if (this.value.length >= 3) {
            telephoneTable.search(this.value).draw();
        }
        else if (this.value == "") {
            telephoneTable.search("").draw();
        }
        return;
    });
}

function telephoneTableContextmenu(userId) {
    $('#telephoneTable').contextMenu({
        selector: 'tbody tr',
        trigger: 'right',
        events: {
            show: function (options) {
                $(this).addClass('selected');
            },
            hide: function (options) {
                $('#telephoneTable tbody tr.selected').removeClass('selected');
            }
        },
        callback: function (key, options) {

            var data = $(this)[0];
            var id = data.id;

            switch (key) {
                case 'add':
                    getTelephoneForm(userId, null);
                    break;
                case 'update':
                    getTelephoneForm(userId, id);
                    break;
                case 'delete':
                    alertify.confirm('Rehber', 'Seçili öğeyi silmek istedüğünüze eminmisiniz ?',
                        function () {
                            $.ajax({
                                type: 'POST',
                                url: "/Telephone/Delete",
                                data: { id },
                                dataType: "json",
                                async: true,
                                success: function (result) {
                                    telephoneTable.ajax.reload(null, false);
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

function getTelephoneForm(userId, id) {
    $.ajax({
        type: 'GET',
        url: "/Telephone/Form",
        data: { userId, id },
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