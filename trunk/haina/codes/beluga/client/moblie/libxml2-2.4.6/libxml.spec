# Note that this is NOT a relocatable package
%define ver      2.4.6
%define prefix   /usr
%define datadir  %{prefix}/share

Summary: Library providing XML and HTML support
Name: libxml2
Version: %ver
Release: 1
Copyright: LGPL
Group: Development/Libraries
Source: ftp://xmlsoft.org/libxml2-%{ver}.tar.gz
BuildRoot: /var/tmp/libxml2-%{PACKAGE_VERSION}-root

URL: http://xmlsoft.org/
Docdir: %{datadir}/doc

%description
This library allows to manipulate XML files. It includes support 
to read, modify and write XML and HTML files. There is DTDs support
this includes parsing and validation even with complex DtDs, either
at parse time or later once the document has been modified. The output
can be a simple SAX stream or and in-memory DOM like representations.
In this case one can use the built-in XPath and XPointer implementation
to select subnodes or ranges. A flexible Input/Output mechanism is
available, with existing HTTP and FTP modules and combined to an
URI library.

%package devel
Summary: Libraries, includes, etc. to develop XML and HTML applications
Group: Development/Libraries
Requires: libxml2 = %{version}

%description devel
Libraries, include files, etc you can use to develop XML applications.
This library allows to manipulate XML files. It includes support 
to read, modify and write XML and HTML files. There is DTDs support
this includes parsing and validation even with complex DtDs, either
at parse time or later once the document has been modified. The output
can be a simple SAX stream or and in-memory DOM like representations.
In this case one can use the built-in XPath and XPointer implementation
to select subnodes or ranges. A flexible Input/Output mechanism is
available, with existing HTTP and FTP modules and combined to an
URI library.


%changelog

* Thu Apr 26 2001 Toshio Kuratomi <badger@prtr-13.ucsc.edu>

[2.3.7]
- Added libxml.m4 to the distribution file list
- Moved the man pages from /usr/man to /usr/share/man to conform to FHS2.0
- Moved programmer documentation into the devel package

* Thu Sep 23 1999 Daniel Veillard <daniel@veillard.com>

- corrected the spec file alpha stuff
- switched to version 1.7.1
- Added validation, XPath, nanohttp, removed memory leaks
- Renamed CHAR to xmlChar

* Wed Jun  2 1999 Daniel Veillard <daniel@veillard.com>

- Switched to version 1.1: SAX extensions, better entities support, lots of
  bug fixes.

* Sun Oct  4 1998 Daniel Veillard <daniel@veillard.com>

- Added xml-config to the package

* Thu Sep 24 1998 Michael Fulbright <msf@redhat.com>

- Built release 0.30

%prep
%setup

%build
# Needed for snapshot releases.
if [ ! -f configure ]; then
%ifarch alpha
  CFLAGS="$RPM_OPT_FLAGS" ./autogen.sh --host=alpha-redhat-linux --prefix=%prefix --sysconfdir="/etc" --mandir=%datadir/man
%else
  CFLAGS="$RPM_OPT_FLAGS" ./autogen.sh --prefix=%prefix --sysconfdir="/etc" --mandir=%datadir/man
%endif
else
%ifarch alpha
  CFLAGS="$RPM_OPT_FLAGS" ./configure --host=alpha-redhat-linux --prefix=%prefix --sysconfdir="/etc" --mandir=%datadir/man
%else
  CFLAGS="$RPM_OPT_FLAGS" ./configure --prefix=%prefix --sysconfdir="/etc" --mandir=%datadir/man
%endif
fi

if [ "$SMP" != "" ]; then
  (make "MAKE=make -k -j $SMP"; exit 0)
  make
else
  make
fi

%install
rm -rf $RPM_BUILD_ROOT

install -d $RPM_BUILD_ROOT%{datadir}/man/man1
install -d $RPM_BUILD_ROOT%{datadir}/man/man4
make prefix=$RPM_BUILD_ROOT%{prefix} mandir=$RPM_BUILD_ROOT%{datadir}/man install

%clean
rm -rf $RPM_BUILD_ROOT

%post -p /sbin/ldconfig

%postun -p /sbin/ldconfig

%files
%defattr(-, root, root)

%doc AUTHORS ChangeLog NEWS README COPYING COPYING.LIB TODO
%doc %{prefix}/share/man/man1/xmllint.1*
%doc %{prefix}/share/man/man1/xmlcatalog.1*
%doc %{prefix}/share/man/man4/libxml.4*

%{prefix}/lib/lib*.so.*
%{prefix}/bin/xmllint
%{prefix}/bin/xmlcatalog

%files devel
%defattr(-, root, root)

%doc /usr/share/man/man1/xml2-config.1*
%doc doc/*.html doc/html

%{prefix}/lib/lib*.so
%{prefix}/lib/*a
%{prefix}/lib/*.sh
%{prefix}/include/*
%{prefix}/bin/xml2-config
%{prefix}/share/aclocal/libxml.m4
%{prefix}/lib/pkgconfig/libxml-2.0.pc
