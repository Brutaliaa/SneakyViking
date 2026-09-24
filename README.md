# Sneaky Viking

I was tired that vanilla sneak skill isn't doing much. This mod basically makes you do less and less [noise](https://valheim.weirdgloop.org/w/Creature_senses) based on your current sneak skill level when walking/running/swimming/dodging. The reduction is configurable in the settings. I recommend using Azus UnOfficial ConfigManager for this, or use your favorite one.

`Version checks with itself. If installed on the server, it will kick clients who do not have it installed.`

`This mod uses ServerSync, if installed on the server and all clients, it will sync all configs to client`

`This mod uses a file watcher. If the configuration file is not changed with BepInEx Configuration manager, but changed in the file directly on the server, upon file save, it will sync the changes to all clients.`

## Configurations

`1- General`

Lock Configuration [Synced with Server]

- If on, the configuration is locked and can be changed by server admins only.
  - Default Value: On

`2- Noise`

Maximum Reduction Percent [Sync with server]

- Maximum reduction% at Sneak skill level 100. If set to 0, it disables it.
  - Default Value: 80

Reduce Dodge Noise [Sync with server]

- Apply the reduction to dodge noise
  - Default Value: On

Reduce Walk/Swim Noise [Sync with server]

- Apply the reduction to walking and swimming noise (They do the same noise)
  - Default Value: On

Reduce Run/Jump Noise [Sync with server]

- Apply the reduction to running and jumping noise.
  - Default Value: On

## Support & Community

**Need Help?**

- You can find me in Azumatt and Hexium discord server (see below)
- Include `LogOutput.log` from BepInEx folder when reporting bugs

### Author: Brutaliaa

**Discord**: Brutaliaa

<table width="25%">
  <tr>
    <td align="center">
      <a href="https://hexium.gg">
        <img
          src="https://hexium.gg/assets/Logo.png"
          alt="Hexium"
          width="64"/>
      </a>
    </td>

<td align="center">
      <a href="https://discord.gg/pdHgy6Bsng">
        <img
          src="https://i.imgur.com/Xlcbmm9.png"
          alt="Azumatt's Discord"
          width="64"/>
      </a>
    </td>
  </tr>
</table>
