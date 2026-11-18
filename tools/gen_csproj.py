"""Regenerates the item groups of LawyerProject.csproj from the files on disk."""
import os, re, sys

root = sys.argv[1]
proj = os.path.join(root, 'LawyerProject.csproj')
s = open(proj, encoding='utf-8-sig').read()
head = s[:s.index('  <ItemGroup>\n    <Reference Include="EntityFramework')]
win = lambda p: p.replace('/', '\\')

files = []
for r, ds, fs in os.walk(root):
    ds[:] = [d for d in ds if d not in ('bin', 'obj')]
    for f in fs:
        files.append(os.path.relpath(os.path.join(r, f), root).replace(os.sep, '/'))
files.sort()

out = [head]
out.append('''  <ItemGroup>
    <Reference Include="EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, processorArchitecture=MSIL">
      <HintPath>..\\packages\\EntityFramework.6.2.0\\lib\\net45\\EntityFramework.dll</HintPath>
    </Reference>
    <Reference Include="EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, processorArchitecture=MSIL">
      <HintPath>..\\packages\\EntityFramework.6.2.0\\lib\\net45\\EntityFramework.SqlServer.dll</HintPath>
    </Reference>
    <Reference Include="System" />
    <Reference Include="System.ComponentModel.DataAnnotations" />
    <Reference Include="System.Configuration" />
    <Reference Include="System.Core" />
    <Reference Include="System.Runtime.Serialization" />
    <Reference Include="System.Security" />
    <Reference Include="System.Transactions" />
    <Reference Include="System.Xml.Linq" />
    <Reference Include="System.Data.DataSetExtensions" />
    <Reference Include="Microsoft.CSharp" />
    <Reference Include="System.Data" />
    <Reference Include="System.Deployment" />
    <Reference Include="System.Drawing" />
    <Reference Include="System.Net.Http" />
    <Reference Include="System.Windows.Forms" />
    <Reference Include="System.Xml" />
  </ItemGroup>
''')

items = []
cs = [f for f in files if f.endswith('.cs')]
forms = set()
for f in cs:
    if f.endswith('.Designer.cs'):
        continue
    if os.path.exists(os.path.join(root, f[:-3] + '.Designer.cs')) and f.startswith('GUI/'):
        forms.add(f)

model_tt = {'User', 'AuditEntry', 'Setting', 'Holiday', 'Customer', 'Court', 'CaseType', 'Procedure', 'CourtCaseType',
            'CaseTypeStep', 'LegalCase', 'CaseStep', 'Payment'}

for f in cs:
    name = os.path.basename(f)[:-3]
    if f in forms:
        items.append('    <Compile Include="%s">\n      <SubType>Form</SubType>\n    </Compile>' % win(f))
    elif f.startswith('GUI/') and name.count('.') == 1 and ('GUI/' + name.split('.')[0] + '.cs') in forms and not f.endswith('.Designer.cs'):
        # a form split into partial files (CaseForm.Payments.cs) nests under the form
        items.append('    <Compile Include="%s">\n      <DependentUpon>%s</DependentUpon>\n    </Compile>' % (win(f), name.split('.')[0] + '.cs'))
    elif f.endswith('.Designer.cs') and f.startswith('GUI/'):
        items.append('    <Compile Include="%s">\n      <DependentUpon>%s</DependentUpon>\n    </Compile>' % (win(f), os.path.basename(f)[:-12] + '.cs'))
    elif f.startswith('DB/') and name in model_tt:
        items.append('    <Compile Include="%s">\n      <DependentUpon>QuedinModel.tt</DependentUpon>\n    </Compile>' % win(f))
    elif f == 'DB/QuedinModel.Context.cs':
        items.append('    <Compile Include="DB\\QuedinModel.Context.cs">\n      <AutoGen>True</AutoGen>\n      <DesignTime>True</DesignTime>\n      <DependentUpon>QuedinModel.Context.tt</DependentUpon>\n    </Compile>')
    elif f == 'DB/QuedinModel.cs':
        items.append('    <Compile Include="DB\\QuedinModel.cs">\n      <AutoGen>True</AutoGen>\n      <DesignTime>True</DesignTime>\n      <DependentUpon>QuedinModel.tt</DependentUpon>\n    </Compile>')
    elif f == 'DB/QuedinModel.Designer.cs':
        items.append('    <Compile Include="DB\\QuedinModel.Designer.cs">\n      <AutoGen>True</AutoGen>\n      <DesignTime>True</DesignTime>\n      <DependentUpon>QuedinModel.edmx</DependentUpon>\n    </Compile>')
    elif f == 'Properties/Resources.Designer.cs':
        items.append('    <Compile Include="Properties\\Resources.Designer.cs">\n      <AutoGen>True</AutoGen>\n      <DesignTime>True</DesignTime>\n      <DependentUpon>Resources.resx</DependentUpon>\n    </Compile>')
    elif f == 'Properties/Settings.Designer.cs':
        items.append('    <Compile Include="Properties\\Settings.Designer.cs">\n      <AutoGen>True</AutoGen>\n      <DependentUpon>Settings.settings</DependentUpon>\n      <DesignTimeSharedInput>True</DesignTimeSharedInput>\n    </Compile>')
    else:
        items.append('    <Compile Include="%s" />' % win(f))

for f in files:
    if f.endswith('.resx'):
        if f == 'Properties/Resources.resx':
            items.append('    <EmbeddedResource Include="Properties\\Resources.resx">\n      <Generator>ResXFileCodeGenerator</Generator>\n      <SubType>Designer</SubType>\n      <LastGenOutput>Resources.Designer.cs</LastGenOutput>\n    </EmbeddedResource>')
        else:
            items.append('    <EmbeddedResource Include="%s">\n      <DependentUpon>%s</DependentUpon>\n    </EmbeddedResource>' % (win(f), os.path.basename(f)[:-5] + '.cs'))

items.append('''    <EntityDeploy Include="DB\\QuedinModel.edmx">
      <Generator>EntityModelCodeGenerator</Generator>
      <LastGenOutput>QuedinModel.Designer.cs</LastGenOutput>
    </EntityDeploy>
    <None Include="DB\\QuedinModel.edmx.diagram">
      <DependentUpon>QuedinModel.edmx</DependentUpon>
    </None>
    <None Include="packages.config" />
    <None Include="Properties\\Settings.settings">
      <Generator>SettingsSingleFileGenerator</Generator>
      <LastGenOutput>Settings.Designer.cs</LastGenOutput>
    </None>
    <None Include="App.config" />
    <Content Include="DB\\QuedinModel.Context.tt">
      <Generator>TextTemplatingFileGenerator</Generator>
      <DependentUpon>QuedinModel.edmx</DependentUpon>
      <LastGenOutput>QuedinModel.Context.cs</LastGenOutput>
    </Content>
    <Content Include="DB\\QuedinModel.tt">
      <Generator>TextTemplatingFileGenerator</Generator>
      <DependentUpon>QuedinModel.edmx</DependentUpon>
      <LastGenOutput>QuedinModel.cs</LastGenOutput>
    </Content>''')
for f in files:
    if f.startswith('Resources/') and not f.endswith('.db'):
        items.append('    <None Include="%s" />' % win(f).replace('(', '%28').replace(')', '%29'))

out.append('  <ItemGroup>\n' + '\n'.join(items) + '\n  </ItemGroup>\n')
out.append('''  <ItemGroup>
    <Service Include="{508349B6-6B84-4DF5-91F0-309BEEBAD82D}" />
  </ItemGroup>
  <Import Project="$(MSBuildToolsPath)\\Microsoft.CSharp.targets" />
</Project>
''')
open(proj, 'w', encoding='utf-8-sig').write(''.join(out))
print('csproj items:', len(items))
