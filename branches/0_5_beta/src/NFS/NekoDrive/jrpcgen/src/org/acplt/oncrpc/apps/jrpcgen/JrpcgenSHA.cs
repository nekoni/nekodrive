namespace org.acplt.oncrpc.apps.jrpcgen
{
	/// <summary>
	/// This class implements the SHA-1 algorithm as described in
	/// "Federal Information Processing Standards Publication 180-1:
	/// Specifications for the Secure Hash Standard.
	/// </summary>
	/// <remarks>
	/// This class implements the SHA-1 algorithm as described in
	/// "Federal Information Processing Standards Publication 180-1:
	/// Specifications for the Secure Hash Standard. April 17, 1995."
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	public class JrpcgenSHA
	{
		/// <summary>Create a new SHA-1 hashing object.</summary>
		/// <remarks>Create a new SHA-1 hashing object.</remarks>
		public JrpcgenSHA()
		{
			reset();
		}

		/// <summary>Update the hash using a single byte (8 bits).</summary>
		/// <remarks>Update the hash using a single byte (8 bits).</remarks>
		/// <param name="b">Byte to hash.</param>
		public virtual void update(byte b)
		{
			int i = (int)bytecount % 64;
			int shift = (3 - i % 4) * 8;
			int idx = i / 4;
			// if you could index ints, this would be: W[idx][shift/8] = b
			W[idx] = (W[idx] & ~(unchecked((int)(0xff)) << shift)) | ((b & unchecked((int)(0xff
				))) << shift);
			// if we've filled up a block, then process it
			if ((++bytecount) % 64 == 0)
			{
				process();
			}
		}

		/// <summary>Update the hash using a short integer (16 bits).</summary>
		/// <remarks>Update the hash using a short integer (16 bits).</remarks>
		/// <param name="s">Short integer to hash.</param>
		public virtual void update(short s)
		{
			update((byte)s);
			update((byte)((s) >> (8 & 0x1f)));
		}

		/// <summary>Update the hash using an integer (32 bits).</summary>
		/// <remarks>Update the hash using an integer (32 bits).</remarks>
		/// <param name="i">Integer to hash.</param>
		public virtual void update(int i)
		{
			update((byte)i);
			update((byte)((i) >> (8 & 0x1f)));
			update((byte)((i) >> (16 & 0x1f)));
			update((byte)((i) >> (24 & 0x1f)));
		}

		/// <summary>Update the hash using a string.</summary>
		/// <remarks>Update the hash using a string.</remarks>
		/// <param name="s">String to hash.</param>
		public virtual void update(string s)
		{
			int len = s.Length;
			for (int idx = 0; idx < len; ++idx)
			{
				update((short)s[idx]);
			}
		}

		/// <summary>
		/// Reset the hashing engine to start hashing another set of innocent
		/// bytes.
		/// </summary>
		/// <remarks>
		/// Reset the hashing engine to start hashing another set of innocent
		/// bytes.
		/// </remarks>
		public virtual void reset()
		{
			bytecount = 0;
			// magic numbers from [1] p. 10.
			H0 = unchecked((int)(0x67452301));
			H1 = unchecked((int)(0xefcdab89));
			H2 = unchecked((int)(0x98badcfe));
			H3 = unchecked((int)(0x10325476));
			H4 = unchecked((int)(0xc3d2e1f0));
		}

		/// <summary>Retrieve the digest (that is, informally spoken, the "hash value").</summary>
		/// <remarks>Retrieve the digest (that is, informally spoken, the "hash value").</remarks>
		/// <returns>digest as a series of 20 bytes (80 bits).</returns>
		public virtual byte[] getDigest()
		{
			long bitcount = bytecount * 8;
			update((byte)unchecked((int)(0x80)));
			// 10000000 in binary; the start of the padding
			// add the rest of the padding to fill this block out, but leave 8
			// bytes to put in the original bytecount
			while ((int)bytecount % 64 != 56)
			{
				update((byte)0);
			}
			// add the length of the original, unpadded block to the end of
			// the padding
			W[14] = (int)((bitcount) >> (32 & 0x1f));
			W[15] = (int)bitcount;
			bytecount += 8;
			// digest the fully padded block
			process();
			byte[] result = new byte[] { (byte)((H0) >> (24 & 0x1f)), (byte)((H0) >> (16 & 0x1f
				)), (byte)((H0) >> (8 & 0x1f)), (byte)H0, (byte)((H1) >> (24 & 0x1f)), (byte)((H1
				) >> (16 & 0x1f)), (byte)((H1) >> (8 & 0x1f)), (byte)H1, (byte)((H2) >> (24 & 0x1f
				)), (byte)((H2) >> (16 & 0x1f)), (byte)((H2) >> (8 & 0x1f)), (byte)H2, (byte)((H3
				) >> (24 & 0x1f)), (byte)((H3) >> (16 & 0x1f)), (byte)((H3) >> (8 & 0x1f)), (byte
				)H3, (byte)((H4) >> (24 & 0x1f)), (byte)((H4) >> (16 & 0x1f)), (byte)((H4) >> (8
				 & 0x1f)), (byte)H4 };
			reset();
			return result;
		}

		/// <summary>
		/// Return first 64 bits of hash digest for use as a serialization
		/// UID, etc.
		/// </summary>
		/// <remarks>
		/// Return first 64 bits of hash digest for use as a serialization
		/// UID, etc.
		/// </remarks>
		/// <returns>hash digest with only 64 bit size.</returns>
		public virtual long getHash()
		{
			byte[] hash = getDigest();
			return (((long)hash[0]) & unchecked((int)(0xFF))) + ((((long)hash[1]) & unchecked(
				(int)(0xFF))) << 8) + ((((long)hash[2]) & unchecked((int)(0xFF))) << 16) + ((((long
				)hash[3]) & unchecked((int)(0xFF))) << 24) + ((((long)hash[4]) & unchecked((int)
				(0xFF))) << 32) + ((((long)hash[5]) & unchecked((int)(0xFF))) << 40) + ((((long)
				hash[6]) & unchecked((int)(0xFF))) << 48) + ((((long)hash[7]) & unchecked((int)(
				0xFF))) << 56);
		}

		/// <summary>Process a single block.</summary>
		/// <remarks>
		/// Process a single block. This is pretty much copied verbatim from
		/// "Federal Information Processing Standards Publication 180-1:
		/// Specifications for the Secure Hash Standard. April 17, 1995.",
		/// pp. 9, 10.
		/// </remarks>
		private void process()
		{
			for (int t = 16; t < 80; ++t)
			{
				int Wt = W[t - 3] ^ W[t - 8] ^ W[t - 14] ^ W[t - 16];
				W[t] = Wt << 1 | (Wt) >> (31 & 0x1f);
			}
			int A = H0;
			int B = H1;
			int C = H2;
			int D = H3;
			int E = H4;
			for (int t = 0; t < 20; ++t)
			{
				int TEMP = (A << 5 | (A) >> (27 & 0x1f)) + ((B & C) | (~B & D)) + E + W[t] + unchecked(
					(int)(0x5a827999));
				// S^5(A)
				// f_t(B,C,D)
				// K_t
				E = D;
				D = C;
				C = B << 30 | (B) >> (2 & 0x1f);
				// S^30(B)
				B = A;
				A = TEMP;
			}
			for (int t = 20; t < 40; ++t)
			{
				int TEMP = (A << 5 | (A) >> (27 & 0x1f)) + (B ^ C ^ D) + E + W[t] + unchecked((int
					)(0x6ed9eba1));
				// S^5(A)
				// f_t(B,C,D)
				// K_t
				E = D;
				D = C;
				C = B << 30 | (B) >> (2 & 0x1f);
				// S^30(B)
				B = A;
				A = TEMP;
			}
			for (int t = 40; t < 60; ++t)
			{
				int TEMP = (A << 5 | (A) >> (27 & 0x1f)) + (B & C | B & D | C & D) + E + W[t] + unchecked(
					(int)(0x8f1bbcdc));
				// S^5(A)
				// f_t(B,C,D)
				// K_t
				E = D;
				D = C;
				C = B << 30 | (B) >> (2 & 0x1f);
				// S^30(B)
				B = A;
				A = TEMP;
			}
			for (int t = 60; t < 80; ++t)
			{
				int TEMP = (A << 5 | (A) >> (27 & 0x1f)) + (B ^ C ^ D) + E + W[t] + unchecked((int
					)(0xca62c1d6));
				// S^5(A)
				// f_t(B,C,D)
				// K_t
				E = D;
				D = C;
				C = B << 30 | (B) >> (2 & 0x1f);
				// S^30(B)
				B = A;
				A = TEMP;
			}
			H0 += A;
			H1 += B;
			H2 += C;
			H3 += D;
			H4 += E;
			// Reset W by clearing it.
			for (int t = 0; t < 80; ++t)
			{
				W[t] = 0;
			}
		}

		/// <summary>Work buffer for calculating the hash.</summary>
		/// <remarks>Work buffer for calculating the hash.</remarks>
		private readonly int[] W = new int[80];

		private long bytecount;

		private int H0;

		private int H1;

		private int H2;

		private int H3;

		private int H4;
	}
}
