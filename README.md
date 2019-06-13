# cshrix [![#cshrix:sharparam.com][matrix-badge]][matrix-cshrix]

[![MPL 2.0 License][mpl-badge]][mpl]
[![Latest GitHub release][ghreleasebadge]][ghrelease]
[![Docker image][docker-badge]][docker]

| [Master][master] | [![Build status][appveyor-master-badge]][appveyor-master-status] | [![TravisCI Status][travis-master-badge]][travis-master-status] | [![Test status][test-master-badge]][test-master-status] | [![Coverage][coveralls-master-badge]][coveralls-master] | [![Codecov coverage][codecov-master-badge]][codecov-master] | [![CodeFactor][codefactor-master-badge]][codefactor-master] |
|-|-|-|-|-|-|-|
| [**Develop**][develop] | [![Build status][appveyor-develop-badge]][appveyor-develop-status] | [![TravisCI Status][travis-develop-badge]][travis-develop-status] | [![Test status][test-develop-badge]][test-develop-status] | [![Coverage][coveralls-develop-badge]][coveralls-develop] | [![Codecov coverage][codecov-develop-badge]][codecov-develop] | [![CodeFactor][codefactor-develop-badge]][codefactor-develop] |

C# library for interacting with the [Matrix][] protocol.

This library is a work in progress!

## Acknowledgements

Big thanks to the almighty [@expectocode][expectocode] for
coming up with a name for this project.

## License

Copyright © 2019 by [Adam Hellberg][sharparam].

This Source Code Form is subject to the terms of the [Mozilla Public
License, v. 2.0][mpl]. If a copy of the MPL was not distributed with this
file, You can obtain one at http://mozilla.org/MPL/2.0/.

See the file [LICENSE][] for more details.

## Code of Conduct

This project is covered under a [Code of Conduct][coc], please remember to
adhere by this when interacting with the project.

## Contributing

Please see the [CONTRIBUTING][] document for our contribution guidelines.

## End-to-end (E2E) encryption

In order for E2EE features to work, [libolm][] must be available on the system
`PATH` or located alongside the executable
(cshrix calls into it using P/Interop).

### Linux

Check if your distro's package manager has [libolm][] available.
It usually goes under the name `libolm` and/or `libolm-dev`.

#### Debian/Ubuntu

Debian: buster/sid and later.

Ubuntu: 18.04 (bionic) and later.

```
# apt install libolm-dev
```

#### Arch

[`libolm` is available in the AUR][libolm-aur] and as a
[git version][libolm-git-aur].


### Windows

As of right now (2019-04-23), no official Windows releases of [libolm][] exist.
Build the library manually and place it either somewhere on the `PATH` or
alongside the executable.

### macOS

Unknown. If someone has information about [libolm][] on macOS, please make
a [PR][] and update this section.

[cshrix]: https://github.com/Sharparam/cshrix
[matrix]: https://matrix.org

[mpl]: https://mozilla.org/MPL/2.0/
[mpl-osi]: https://opensource.org/licenses/MPL-2.0
[mpl-badge]: https://img.shields.io/badge/license-MPL%202.0-blue.svg

[license]: LICENSE
[coc]: CODE_OF_CONDUCT.md
[contributing]: CONTRIBUTING.md

[sharparam]: https://github.com/Sharparam
[expectocode]: https://github.com/expectocode

[master]: https://github.com/Sharparam/cshrix/tree/master
[develop]: https://github.com/Sharparam/cshrix/tree/develop
[pr]: https://github.com/Sharparam/cshrix/pulls

[libolm]: https://gitlab.matrix.org/matrix-org/olm
[libolm-aur]: https://aur.archlinux.org/packages/libolm/
[libolm-git-aur]: https://aur.archlinux.org/packages/libolm-git/

[matrix-sharparam]: https://matrix.to/#/@sharparam:sharparam.com
[matrix-cshrix]: https://matrix.to/#/!FWdevnnPQnzpNpaUJf:sharparam.com?via=sharparam.com
[matrix-badge]: https://img.shields.io/matrix/cshrix:sharparam.com.svg?label=%23cshrix%3Asharparam.com&logo=matrix&server_fqdn=sharparam.com

[ghrelease]: https://github.com/Sharparam/cshrix/releases
[ghreleasebadge]: https://img.shields.io/github/release/Sharparam/cshrix.svg?logo=github

[appveyor-develop-status]: https://ci.appveyor.com/project/Sharparam/cshrix/branch/develop
[appveyor-develop-badge]: https://ci.appveyor.com/api/projects/status/s7o8vwq4udo1iudk/branch/develop?svg=true
[travis-develop-status]: https://travis-ci.com/Sharparam/cshrix
[travis-develop-badge]: https://travis-ci.com/Sharparam/cshrix.svg?branch=develop
[test-develop-status]: https://ci.appveyor.com/project/Sharparam/cshrix/branch/develop/tests
[test-develop-badge]: https://img.shields.io/appveyor/tests/Sharparam/cshrix/develop.svg
[coveralls-develop]: https://coveralls.io/github/Sharparam/cshrix?branch=develop
[coveralls-develop-badge]: https://coveralls.io/repos/github/Sharparam/cshrix/badge.svg?branch=develop
[codecov-develop]: https://codecov.io/gh/Sharparam/cshrix/branch/develop
[codecov-develop-badge]: https://codecov.io/gh/Sharparam/cshrix/branch/develop/graph/badge.svg
[codefactor-develop]: https://www.codefactor.io/repository/github/sharparam/cshrix/overview/develop
[codefactor-develop-badge]: https://www.codefactor.io/repository/github/sharparam/cshrix/badge/develop

[appveyor-master-status]: https://ci.appveyor.com/project/Sharparam/cshrix/branch/master
[appveyor-master-badge]: https://ci.appveyor.com/api/projects/status/s7o8vwq4udo1iudk/branch/master?svg=true
[travis-master-status]: https://travis-ci.com/Sharparam/cshrix
[travis-master-badge]: https://travis-ci.com/Sharparam/cshrix.svg?branch=master
[test-master-status]: https://ci.appveyor.com/project/Sharparam/cshrix/branch/master/tests
[test-master-badge]: https://img.shields.io/appveyor/tests/Sharparam/cshrix/master.svg
[coveralls-master]: https://coveralls.io/github/Sharparam/cshrix
[coveralls-master-badge]: https://coveralls.io/repos/github/Sharparam/cshrix/badge.svg
[codecov-master]: https://codecov.io/gh/Sharparam/cshrix
[codecov-master-badge]: https://codecov.io/gh/Sharparam/cshrix/branch/master/graph/badge.svg
[codefactor-master]: https://www.codefactor.io/repository/github/sharparam/cshrix/overview/master
[codefactor-master-badge]: https://www.codefactor.io/repository/github/sharparam/cshrix/badge/master
