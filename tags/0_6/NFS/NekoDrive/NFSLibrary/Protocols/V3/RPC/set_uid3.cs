/*
 * Automatically generated by jrpcgen 1.0.7 on 27/08/2010
 * jrpcgen is part of the "Remote Tea.Net" ONC/RPC package for C#
 * See http://remotetea.sourceforge.net for details
 */
using org.acplt.oncrpc;

public class set_uid3 : XdrAble {
    public bool set_it;
    public uid3 uid;

    public set_uid3() {
    }

    public set_uid3(XdrDecodingStream xdr) {
        xdrDecode(xdr);
    }

    public void xdrEncode(XdrEncodingStream xdr) {
        xdr.xdrEncodeBoolean(set_it);
        if ( set_it == true ) {
            uid.xdrEncode(xdr);
        }
    }

    public void xdrDecode(XdrDecodingStream xdr) {
        set_it = xdr.xdrDecodeBoolean();
        if ( set_it == true ) {
            uid = new uid3(xdr);
        }
    }

}
// End of set_uid3.cs