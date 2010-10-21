/*
 * Automatically generated by jrpcgen 1.0.7 on 27/08/2010
 * jrpcgen is part of the "Remote Tea.Net" ONC/RPC package for C#
 * See http://remotetea.sourceforge.net for details
 */
using org.acplt.oncrpc;

public class wcc_data : XdrAble {
    public pre_op_attr before;
    public post_op_attr after;

    public wcc_data() {
    }

    public wcc_data(XdrDecodingStream xdr) {
        xdrDecode(xdr);
    }

    public void xdrEncode(XdrEncodingStream xdr) {
        before.xdrEncode(xdr);
        after.xdrEncode(xdr);
    }

    public void xdrDecode(XdrDecodingStream xdr) {
        before = new pre_op_attr(xdr);
        after = new post_op_attr(xdr);
    }

}
// End of wcc_data.cs