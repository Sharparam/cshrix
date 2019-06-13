#!/usr/bin/env ruby

# Copyright (c) 2019 by Adam Hellberg.
#
# This Source Code Form is subject to the terms of the Mozilla Public
# License, v. 2.0. If a copy of the MPL was not distributed with this
# file, You can obtain one at http://mozilla.org/MPL/2.0/.

require 'time'

BOM = "\u00ef\u00bb\u00bf"
START_YEAR = 2019
CURRENT_YEAR = Time.now.year

YEARS = CURRENT_YEAR == START_YEAR ? START_YEAR : "#{START_YEAR}-#{CURRENT_YEAR}"

TEMPLATE = <<END
Copyright (c) #{YEARS} by Adam Hellberg.

This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
file, You can obtain one at http://mozilla.org/MPL/2.0/.
END

FORMATTERS = {
  cs: -> (filename, tmpl) do
    lines = ["// <copyright file=\"#{filename}\">\n"]
    lines.concat tmpl.lines.map { |line| line.strip.empty? ? "//\n" : "//   #{line}" }
    lines << "// </copyright>\n"
  end,
  xml: -> (filename, tmpl) do
    tmpl.lines.map.with_index do |line, i|
      if line.strip.empty?
        "   -\n"
      elsif i == 0
        "<!-- #{line}"
      else
        "   - #{line}"
      end
    end.tap do |mapped|
      mapped[-1] = "#{mapped[-1].chomp} -->\n"
    end
  end
}.freeze

COMMENT_CHECKS = {
  cs: -> line { line.start_with? '//' },
  xml: -> line { line.start_with? '<!--' }
}.freeze

FILE_EXT_PATTERNS = {
  /\.cs$/ => :cs,
  /\.cake$/ => :cs,
  /\.xml$/ => :xml,
  /\.csproj$/ => :xml
}.freeze

EXCLUDE = [
  /\.idea/,
  /\.git/
].freeze

# Simple check for now: If file contains a comment on the first line, then
# assume it has a copyright notice.
def has_copyright?(file, type)
  first_line = File.open(file, 'r:bom|utf-8', &:readline)
  COMMENT_CHECKS[type].call(first_line)
end

def patch!(file, type)
  puts "Patching [#{type}] #{file}"

  lines = FORMATTERS[type].call(File.basename(file), TEMPLATE)
  lines << "\n"
  lines.concat File.readlines(file, encoding: 'bom|utf-8')
  content = lines.join
  File.write(file, content, encoding: 'utf-8')
end

files = `git ls-files`.split("\n").reject { |f| EXCLUDE.any? { |e| e.match? f } }

files.each do |file|
  type = FILE_EXT_PATTERNS.find { |(k, _)| k.match? file }&.last
  next unless type
  patch! file, type unless has_copyright? file, type
end
