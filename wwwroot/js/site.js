window.extrasJS = {
    SetCookie: function (cname, cvalue) {
        var d = new Date();
        d.setTime(d.getTime() + (30 * 60 * 1000)); // 30 minutos en milisegundos
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    },

    //SetCookie: function (cname, cvalue, exdays) { var d = new Date(); d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000)); var expires = "expires=" + d.toUTCString(); document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/"; },
    DeleteCookie: function (cname)
    {
        var d = new Date();
        d.setTime(d.getTime() - (365 * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + ";" + expires + ";path=/";
    },

    GetCookie: function (cname)
    {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++)
        {
            var c = ca[i];
            while (c.charAt(0) == ' ')
            { c = c.substring(1); }
            if (c.indexOf(name) == 0)
            {
                return c.substring(name.length, c.length);
            }
        } return "";
    },
}


